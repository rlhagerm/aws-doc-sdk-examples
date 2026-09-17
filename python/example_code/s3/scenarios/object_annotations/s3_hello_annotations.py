# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Hello S3 Object Annotations — a standalone example that lists annotations
on a specified S3 object, demonstrating the simplest use of the feature.

Usage:
    python s3_hello_annotations.py <bucket-name> <object-key>
"""

# snippet-start:[python.example_code.s3.Hello_ObjectAnnotations]
import logging
import sys

import boto3
from botocore.exceptions import ClientError

logger = logging.getLogger(__name__)


def hello_annotations(bucket_name: str, object_key: str) -> None:
    """
    Lists annotations for an S3 object and displays their details.

    :param bucket_name: The name of the bucket containing the object.
    :param object_key: The key of the object.
    """
    s3_client = boto3.client("s3")

    print(
        f"\nListing annotations for object '{object_key}' "
        f"in bucket '{bucket_name}'..."
    )

    try:
        paginator = s3_client.get_paginator("list_object_annotations")
        annotations = list()
        for page in paginator.paginate(Bucket=bucket_name, Key=object_key):
            for annotation in page.get("Annotations", list()):
                annotations.append(annotation)

        if annotations:
            print(f"Found {len(annotations)} annotation(s):")
            for ann in annotations:
                name = ann.get("AnnotationName", "")
                size = ann.get("Size", 0)
                etag = ann.get("ETag", "")
                last_modified = ann.get("LastModified", None)
                print(
                    f'  - "{name}" ({size} bytes, '
                    f"last modified: {last_modified})"
                )
        else:
            print(f"Object '{object_key}' has no annotations.")

    except ClientError as err:
        if err.response["Error"]["Code"] == "NoSuchKey":
            print(
                f"Error: Object '{object_key}' does not exist "
                f"in bucket '{bucket_name}'."
            )
        else:
            logger.error(
                "Error listing annotations: %s", err.response["Error"]["Message"]
            )
            raise


if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python s3_hello_annotations.py <bucket-name> <object-key>")
        sys.exit(1)
    logging.basicConfig(level=logging.INFO)
    hello_annotations(sys.argv[1], sys.argv[2])
# snippet-end:[python.example_code.s3.Hello_ObjectAnnotations]
