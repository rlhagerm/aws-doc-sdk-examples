# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Integration tests for the Amazon S3 Object Annotations scenario.

These tests exercise the S3AnnotationsWrapper against real AWS resources.
They do NOT mock the S3 client.
"""

import json
import random
import string

import boto3
import pytest
from botocore.exceptions import ClientError

from s3_wrapper import S3AnnotationsWrapper


def _random_suffix(length: int = 8) -> str:
    """Generates a random lowercase alphanumeric suffix."""
    return "".join(random.choices(string.ascii_lowercase + string.digits, k=length))


@pytest.fixture(name="s3_wrapper")
def fixture_s3_wrapper():
    """Creates an S3AnnotationsWrapper using a real Boto3 S3 client."""
    client = boto3.client("s3")
    return S3AnnotationsWrapper(client)


@pytest.fixture(name="test_bucket")
def fixture_test_bucket(s3_wrapper):
    """Creates a test bucket and deletes it after the test."""
    bucket_name = f"integ-test-annotations-{_random_suffix()}"
    s3_wrapper.create_bucket(bucket_name)
    yield bucket_name
    # Cleanup: delete all objects and the bucket.
    try:
        s3_client = s3_wrapper.s3_client
        response = s3_client.list_objects_v2(Bucket=bucket_name)
        for obj in response.get("Contents", list()):
            s3_client.delete_object(Bucket=bucket_name, Key=obj["Key"])
        s3_client.delete_bucket(Bucket=bucket_name)
    except ClientError:
        pass


@pytest.fixture(name="test_object")
def fixture_test_object(s3_wrapper, test_bucket):
    """Uploads a test object to the test bucket."""
    object_key = "integ-test-object.txt"
    s3_wrapper.put_object(test_bucket, object_key, "Integration test content.")
    return object_key


@pytest.mark.integ
class TestS3AnnotationsWrapper:
    """Integration tests for S3AnnotationsWrapper methods."""

    def test_create_bucket_and_put_object(self, s3_wrapper, test_bucket, test_object):
        """Tests bucket creation and object upload."""
        # If we got here, the fixtures succeeded. Verify the object exists.
        response = s3_wrapper.s3_client.head_object(
            Bucket=test_bucket, Key=test_object
        )
        assert response["ResponseMetadata"]["HTTPStatusCode"] == 200

    def test_put_and_get_object_annotation(
        self, s3_wrapper, test_bucket, test_object
    ):
        """Tests putting and getting an annotation."""
        annotation_name = "test-annotation"
        payload = json.dumps({"key": "value", "number": 42})

        try:
            # Put annotation
            put_response = s3_wrapper.put_object_annotation(
                test_bucket, test_object, annotation_name, payload
            )
            assert "ETag" in put_response

            # Get annotation
            result = s3_wrapper.get_object_annotation(
                test_bucket, test_object, annotation_name
            )
            assert result["Payload"] == payload
            assert result["ContentLength"] > 0
            assert result["ETag"] is not None
        finally:
            try:
                s3_wrapper.delete_object_annotation(
                    test_bucket, test_object, annotation_name
                )
            except ClientError:
                pass

    def test_list_object_annotations(self, s3_wrapper, test_bucket, test_object):
        """Tests listing annotations with and without prefix filtering."""
        annotations_to_create = [
            ("prefix/ann-1", '{"data": 1}'),
            ("prefix/ann-2", '{"data": 2}'),
            ("other-ann", '{"data": 3}'),
        ]

        try:
            # Create annotations
            for name, payload in annotations_to_create:
                s3_wrapper.put_object_annotation(
                    test_bucket, test_object, name, payload
                )

            # List all
            all_annotations = s3_wrapper.list_object_annotations(
                test_bucket, test_object
            )
            assert len(all_annotations) >= 3

            # List with prefix
            prefix_annotations = s3_wrapper.list_object_annotations(
                test_bucket, test_object, annotation_prefix="prefix/"
            )
            assert len(prefix_annotations) >= 2
            for ann in prefix_annotations:
                assert ann["AnnotationName"].startswith("prefix/")
        finally:
            for name, _ in annotations_to_create:
                try:
                    s3_wrapper.delete_object_annotation(
                        test_bucket, test_object, name
                    )
                except ClientError:
                    pass

    def test_update_annotation(self, s3_wrapper, test_bucket, test_object):
        """Tests updating an annotation by overwriting it."""
        annotation_name = "updatable-annotation"
        original_payload = '{"version": 1}'
        updated_payload = '{"version": 2}'

        try:
            # Create original
            s3_wrapper.put_object_annotation(
                test_bucket, test_object, annotation_name, original_payload
            )

            # Overwrite
            s3_wrapper.put_object_annotation(
                test_bucket, test_object, annotation_name, updated_payload
            )

            # Verify
            result = s3_wrapper.get_object_annotation(
                test_bucket, test_object, annotation_name
            )
            assert result["Payload"] == updated_payload
        finally:
            try:
                s3_wrapper.delete_object_annotation(
                    test_bucket, test_object, annotation_name
                )
            except ClientError:
                pass

    def test_delete_object_annotation(self, s3_wrapper, test_bucket, test_object):
        """Tests deleting an annotation and confirms NoSuchAnnotation error."""
        annotation_name = "delete-me-annotation"
        payload = '{"temporary": true}'

        try:
            # Create
            s3_wrapper.put_object_annotation(
                test_bucket, test_object, annotation_name, payload
            )

            # Delete
            s3_wrapper.delete_object_annotation(
                test_bucket, test_object, annotation_name
            )

            # Verify it's gone
            with pytest.raises(ClientError) as exc_info:
                s3_wrapper.get_object_annotation(
                    test_bucket, test_object, annotation_name
                )
            assert exc_info.value.response["Error"]["Code"] == "NoSuchAnnotation"
        finally:
            # Best-effort cleanup (annotation should already be deleted)
            try:
                s3_wrapper.delete_object_annotation(
                    test_bucket, test_object, annotation_name
                )
            except ClientError:
                pass

    def test_list_annotations_nosuchkey_error(self, s3_wrapper, test_bucket):
        """Tests that listing annotations on a nonexistent object raises NoSuchKey."""
        with pytest.raises(ClientError) as exc_info:
            s3_wrapper.list_object_annotations(
                test_bucket, "nonexistent-object-key.txt"
            )
        assert exc_info.value.response["Error"]["Code"] == "NoSuchKey"

    def test_get_annotation_nosuchannotation_error(
        self, s3_wrapper, test_bucket, test_object
    ):
        """Tests that getting a nonexistent annotation raises NoSuchAnnotation."""
        with pytest.raises(ClientError) as exc_info:
            s3_wrapper.get_object_annotation(
                test_bucket, test_object, "nonexistent-annotation"
            )
        assert exc_info.value.response["Error"]["Code"] == "NoSuchAnnotation"

    def test_full_lifecycle(self, s3_wrapper, test_bucket, test_object):
        """End-to-end lifecycle: put, get, list, update, delete, verify removal."""
        ann_name = "lifecycle-test"
        payload_v1 = '{"step": 1}'
        payload_v2 = '{"step": 2}'

        try:
            # Put
            s3_wrapper.put_object_annotation(
                test_bucket, test_object, ann_name, payload_v1
            )

            # Get
            result = s3_wrapper.get_object_annotation(
                test_bucket, test_object, ann_name
            )
            assert result["Payload"] == payload_v1

            # List
            annotations = s3_wrapper.list_object_annotations(
                test_bucket, test_object
            )
            names = [a["AnnotationName"] for a in annotations]
            assert ann_name in names

            # Update
            s3_wrapper.put_object_annotation(
                test_bucket, test_object, ann_name, payload_v2
            )
            result = s3_wrapper.get_object_annotation(
                test_bucket, test_object, ann_name
            )
            assert result["Payload"] == payload_v2

            # Delete
            s3_wrapper.delete_object_annotation(
                test_bucket, test_object, ann_name
            )

            # Verify deleted
            with pytest.raises(ClientError) as exc_info:
                s3_wrapper.get_object_annotation(
                    test_bucket, test_object, ann_name
                )
            assert exc_info.value.response["Error"]["Code"] == "NoSuchAnnotation"
        finally:
            try:
                s3_wrapper.delete_object_annotation(
                    test_bucket, test_object, ann_name
                )
            except ClientError:
                pass
