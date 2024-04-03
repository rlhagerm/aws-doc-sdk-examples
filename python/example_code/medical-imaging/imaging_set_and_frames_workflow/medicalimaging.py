# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0
import concurrent
import logging
import boto3
import os
import gzip
import json
import jmespath
import numpy as np
import zlib
import pydicom
import dicom2jpg


from botocore.exceptions import ClientError

logger = logging.getLogger(__name__)


# snippet-start:[python.example_code.medical-imaging.MedicalImagingWrapper.class]
# snippet-start:[python.example_code.medical-imaging.MedicalImagingWrapper.decl]
class ImageFrameInfo:
    pass


class MedicalImagingWrapper:
    """Encapsulates Amazon HealthImaging functionality."""

    def __init__(self, medical_imaging_client):
        """
        :param medical_imaging_client: A Boto3 Amazon MedicalImaging client.
        """
        self.medical_imaging_client = medical_imaging_client

    @classmethod
    def from_client(cls):
        medical_imaging_client = boto3.client("medical-imaging")
        return cls(medical_imaging_client)

    # snippet-end:[python.example_code.medical-imaging.MedicalImagingWrapper.decl]

    def download_decode_and_check_image_frames(
            self,
            data_store_id,
            image_frames,
            out_directory):
        """
        Downloads image frames, decodes them, and uses the checksum to validate
        the decoded images.

        :param data_store_id: The HealthImaging data store ID.
        :param image_frames: A list of dicts containing image frame information.
        :param out_directory: A directory for the downloaded images.
        :return: True if the function succeeded; otherwise, False.
        """

        # Test with a single file
        image_frame = image_frames[0]
        image_file_path = f"{out_directory}/image_{image_frame['ImageSetId']}.jpg"
        result = self.get_pixel_data(
            image_file_path,
            data_store_id,
            image_frame['ImageSetId'],
            image_frame['ImageFrameId'])

        return result
        """
        semaphore = concurrent.futures.Semaphore(25)
        result = True
        futures = []

        with concurrent.futures.ThreadPoolExecutor(max_workers=25) as executor:
            for image_frame in image_frames:
                get_image_frame_request = {
                    'DatastoreId': data_store_id,
                    'ImageSetId': image_frame['ImageSetId'],
                    'ImageFrameInformation': {'ImageFrameId': image_frame['ImageFrameId']}
                }

                future = executor.submit(
                    self._download_and_decode_image_frame,
                    semaphore, get_image_frame_request, out_directory, image_frame)
                futures.append(future)

            for future in concurrent.futures.as_completed(futures):
                try:
                    res = future.result()
                except Exception as e:
                    logger.error(f"Error while downloading image frame: {e}")
                    result = False

        if result:
            print(f"{len(image_frames)} image files were downloaded.")

        return result
        """

    def _download_and_decode_image_frame(
            self,
            semaphore,
            get_image_frame_request,
            out_directory: str,
            image_frame):
        """
        Downloads and decodes a single image frame.

        :param semaphore: A semaphore to limit concurrent operations.
        :param get_image_frame_request: The request to get the image frame.
        :param out_directory: The directory to save the downloaded image.
        :param image_frame: A dict containing the image frame information.
        :return: True if the operation succeeded; otherwise, False.
        """
        with semaphore:
            try:
                response = self.medical_imaging_client.get_image_frame(**get_image_frame_request)
                # Decode and save the image frame here
                output_image = self.ph_image_to_opj_bitmap(response["imageFrameBlob"])
                ...
            except ClientError as err:
                logger.error(
                    "Couldn't download image frame %s from image set %s. "
                    "Here's why: %s: %s", image_frame['ImageFrameId'],
                    image_frame['ImageSetId'], err.response['Error']['Code'],
                    err.response['Error']['Message'])
                return False
            else:
                return True

    def verifyChecksumForImage(self, image, crc32Checksum):
        channels = image.numcomps
        result = False
        if channels > 0:
            # Assume precision is same for all channels
            precision = image.comps[0].prec
            signedData = image.comps[0].sgnd
            bytes = (precision + 7) // 8

            if signedData:
                if bytes == 1:
                    result = self.verifyChecksumForImageForType(image, crc32Checksum, np.int8)
                elif bytes == 2:
                    result = self.verifyChecksumForImageForType(image, crc32Checksum, np.int16)
                elif bytes == 4:
                    result = self.verifyChecksumForImageForType(image, crc32Checksum, np.int32)
                else:
                    print(f"Unsupported signed data type with {bytes} bytes")

            else:
                if bytes == 1:
                    result = self.verifyChecksumForImageForType(image, crc32Checksum, np.uint8)
                elif bytes == 2:
                    result = self.verifyChecksumForImageForType(image, crc32Checksum, np.uint16)
                elif bytes == 4:
                    result = self.erifyChecksumForImageForType(image, crc32Checksum, np.uint32)
                else:
                    print(f"Unsupported unsigned data type with {bytes} bytes.")
        else:
            print("No channels in image.")
        return result;

    @staticmethod
    def verify_checksum_for_image_for_type(self, image, crc32_checksum, dtype):
        width = image.x1 - image.x0
        height = image.y1 - image.y0
        num_of_channels = image.numcomps

        # Buffer for interleaved bitmap
        buffer = np.zeros((width * height * num_of_channels,), dtype)

        # Convert planar bitmap to interleaved bitmap
        for channel in range(num_of_channels):
            for row in range(height):
                from_row_start = row // image.comps[channel].dy * width // image.comps[channel].dx
                to_index = (row * width) * num_of_channels + channel

                for col in range(width):
                    from_index = from_row_start + col // image.comps[channel].dx

                    buffer[to_index] = image.comps[channel].data[from_index]

                    to_index += num_of_channels

        # Verify checksum
        crc32 = zlib.crc32(buffer)

        result = crc32 == crc32_checksum
        if not result:
            print(f"Checksum mismatch, expected {crc32_checksum}, actual {crc32}")

        return result

    @staticmethod
    def jph_image_to_opj_bitmap(jph_file):
        """
        Decode
        :param jph_file:
        """
        dicom2jpg.dicom2jpg(jph_file)

    # snippet-start:[python.example_code.medical-imaging.CreateDatastore]
    def create_datastore(self, name):
        """
        Create a data store.

        :param name: The name of the data store to create.
        :return: The data store ID.
        """
        try:
            data_store = self.medical_imaging_client.create_datastore(datastoreName=name)
        except ClientError as err:
            logger.error(
                "Couldn't create data store %s. Here's why: %s: %s",
                name,
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise
        else:
            return data_store["datastoreId"]

        # snippet-end:[python.example_code.medical-imaging.CreateDatastore]

        # snippet-start:[python.example_code.medical-imaging.GetDatastore]

    def get_datastore_properties(self, datastore_id):
        """
        Get the properties of a data store.

        :param datastore_id: The ID of the data store.
        :return: The data store properties.
        """
        try:
            data_store = self.medical_imaging_client.get_datastore(
                datastoreId=datastore_id
            )
        except ClientError as err:
            logger.error(
                "Couldn't get data store %s. Here's why: %s: %s",
                id,
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise
        else:
            return data_store["datastoreProperties"]

    # snippet-end:[python.example_code.medical-imaging.GetDatastore]

    # snippet-start:[python.example_code.medical-imaging.DeleteDatastore]

    def delete_datastore(self, datastore_id):
        """
        Delete a data store.

        :param datastore_id: The ID of the data store.
        """
        try:
            self.medical_imaging_client.delete_datastore(datastoreId=datastore_id)
        except ClientError as err:
            logger.error(
                "Couldn't delete data store %s. Here's why: %s: %s",
                datastore_id,
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise

        # snippet-end:[python.example_code.medical-imaging.DeleteDatastore]

        # snippet-start:[python.example_code.medical-imaging.StartDICOMImportJob]

    def start_dicom_import_job(self, data_store_id, input_bucket_name, input_directory,
                               output_bucket_name, output_directory, role_arn):
        """
        Routine which starts a HealthImaging import job.

        :param data_store_id: The HealthImaging data store ID.
        :param input_bucket_name: The name of the Amazon S3 bucket containing the DICOM files.
        :param input_directory: The directory in the S3 bucket containing the DICOM files.
        :param output_bucket_name: The name of the S3 bucket for the output.
        :param output_directory: The directory in the S3 bucket to store the output.
        :param role_arn: The ARN of the IAM role with permissions for the import.
        :return: The job ID of the import.
        """

        input_uri = f"s3://{input_bucket_name}/{input_directory}/"
        output_uri = f"s3://{output_bucket_name}/{output_directory}/"

        response = self.medical_imaging_client.start_dicom_import_job(
            datastoreId=data_store_id,
            dataAccessRoleArn=role_arn,
            inputS3Uri=input_uri,
            outputS3Uri=output_uri
        )

        if 'JobId' in response:
            import_job_id = response['JobId']
            return import_job_id
        else:
            print(f"Failed to start DICOM import job because {response['Error']['Message']}")
            return False

    def start_dicom_import_job_old(
            self, job_name, datastore_id, role_arn, input_s3_uri, output_s3_uri
    ):
        """
        Start a DICOM import job.

        :param job_name: The name of the job.
        :param datastore_id: The ID of the data store.
        :param role_arn: The Amazon Resource Name (ARN) of the role to use for the job.
        :param input_s3_uri: The S3 bucket input prefix path containing the DICOM files.
        :param output_s3_uri: The S3 bucket output prefix path for the result.
        :return: The job ID.
        """
        try:
            job = self.medical_imaging_client.start_dicom_import_job(
                jobName=job_name,
                datastoreId=datastore_id,
                dataAccessRoleArn=role_arn,
                inputS3Uri=input_s3_uri,
                outputS3Uri=output_s3_uri,
            )
        except ClientError as err:
            logger.error(
                "Couldn't start DICOM import job. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise
        else:
            return job["jobId"]

        # snippet-end:[python.example_code.medical-imaging.StartDICOMImportJob]

        # snippet-start:[python.example_code.medical-imaging.GetDICOMImportJob]

    def get_dicom_import_job(self, datastore_id, job_id):
        """
        Get the properties of a DICOM import job.

        :param datastore_id: The ID of the data store.
        :param job_id: The ID of the job.
        :return: The job properties.
        """
        try:
            job = self.medical_imaging_client.get_dicom_import_job(
                jobId=job_id, datastoreId=datastore_id
            )
        except ClientError as err:
            logger.error(
                "Couldn't get DICOM import job. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise
        else:
            return job["jobProperties"]

        # snippet-end:[python.example_code.medical-imaging.GetDICOMImportJob]

        # snippet-start:[python.example_code.medical-imaging.ListDICOMImportJobs]

    def list_dicom_import_jobs(self, datastore_id):
        """
        List the DICOM import jobs.

        :param datastore_id: The ID of the data store.
        :return: The list of jobs.
        """
        try:
            paginator = self.medical_imaging_client.get_paginator(
                "list_dicom_import_jobs"
            )
            page_iterator = paginator.paginate(datastoreId=datastore_id)
            job_summaries = []
            for page in page_iterator:
                job_summaries.extend(page["jobSummaries"])
        except ClientError as err:
            logger.error(
                "Couldn't list DICOM import jobs. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise
        else:
            return job_summaries

        # snippet-end:[python.example_code.medical-imaging.ListDICOMImportJobs]

        # snippet-start:[python.example_code.medical-imaging.SearchImageSets]

    def search_image_sets(self, datastore_id, search_filter):
        """
        Search for image sets.

        :param datastore_id: The ID of the data store.
        :param search_filter: The search filter.
            For example: {"filters" : [{ "operator": "EQUAL", "values": [{"DICOMPatientId": "3524578"}]}]}.
        :return: The list of image sets.
        """
        try:
            paginator = self.medical_imaging_client.get_paginator("search_image_sets")
            page_iterator = paginator.paginate(
                datastoreId=datastore_id, searchCriteria=search_filter
            )
            metadata_summaries = []
            for page in page_iterator:
                metadata_summaries.extend(page["imageSetsMetadataSummaries"])
        except ClientError as err:
            logger.error(
                "Couldn't search image sets. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise
        else:
            return metadata_summaries

        # snippet-end:[python.example_code.medical-imaging.SearchImageSets]

        # snippet-start:[python.example_code.medical-imaging.GetImageSet]

    def get_image_frames_for_image_set(self, datastore_id, image_set_id, out_directory):
        """
        Get the image frames for an image set.

        :param datastore_id: The ID of the data store.
        :param image_set_id: The ID of the image set.
        :param out_directory: The optional version of the image set.
        :return: The image frames.
        """
        image_frames = []
        file_name = os.path.join(out_directory, f"{image_set_id}_metadata.json.gzip")
        if self.get_image_set_metadata(datastore_id, image_set_id, "", file_name):
            try:
                with gzip.open(file_name, 'r') as f:
                    metadata_gzip = f.read()
                metadata_json = gzip.decompress(metadata_gzip).decode('utf-8')
                doc = json.loads(metadata_json)
                instances = jmespath.search("Study.Series.*.Instances[].*[]", doc)
                for instance in instances:
                    rescale_slope = jmespath.search("DICOM.RescaleSlope", instance)
                    rescale_intercept = jmespath.search("DICOM.RescaleIntercept", instance)
                    image_frames_json = jmespath.search("ImageFrames[][]", instance)
                    for image_frame in image_frames_json:
                        image_frame_info = ImageFrameInfo()
                        image_frame_info.image_set_id = image_set_id
                        image_frame_info.image_frame_id = image_frame["ID"]
                        image_frame_info.rescale_intercept = rescale_intercept
                        image_frame_info.rescale_slope = rescale_slope
                        image_frame_info.min_pixel_value = image_frame["MinPixelValue"]
                        image_frame_info.max_pixel_value = image_frame["MaxPixelValue"]
                        checksum_json = jmespath.search(
                            "max_by(PixelDataChecksumFromBaseToFullResolution, &Width).Checksum", image_frame)
                        image_frame_info.full_resolution_checksum = checksum_json
                        image_frames.append(image_frame_info)
                return image_frames
            except ClientError as err:
                logger.error(
                    "Couldn't get image frames for image set. Here's why: %s: %s",
                    err.response["Error"]["Code"],
                    err.response["Error"]["Message"],
                )
                raise
        else:
            return image_frames

    def get_image_set(self, datastore_id, image_set_id, version_id=None):
        """
        Get the properties of an image set.

        :param datastore_id: The ID of the data store.
        :param image_set_id: The ID of the image set.
        :param version_id: The optional version of the image set.
        :return: The image set properties.
        """
        try:
            if version_id:
                image_set = self.medical_imaging_client.get_image_set(
                    imageSetId=image_set_id,
                    datastoreId=datastore_id,
                    versionId=version_id,
                )
            else:
                image_set = self.medical_imaging_client.get_image_set(
                    imageSetId=image_set_id, datastoreId=datastore_id
                )
        except ClientError as err:
            logger.error(
                "Couldn't get image set. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise
        else:
            return image_set

        # snippet-end:[python.example_code.medical-imaging.GetImageSet]

        # snippet-start:[python.example_code.medical-imaging.GetImageFrame]

    def get_pixel_data(
            self, file_path_to_write, datastore_id, image_set_id, image_frame_id
    ):
        """
        Get an image frame's pixel data.

        :param file_path_to_write: The path to write the image frame's HTJ2K encoded pixel data.
        :param datastore_id: The ID of the data store.
        :param image_set_id: The ID of the image set.
        :param image_frame_id: The ID of the image frame.
        """
        try:
            image_frame = self.medical_imaging_client.get_image_frame(
                datastoreId=datastore_id,
                imageSetId=image_set_id,
                imageFrameInformation={"imageFrameId": image_frame_id},
            )
            with open(file_path_to_write, "wb") as f:
                for chunk in image_frame["imageFrameBlob"].iter_chunks():
                    if chunk:
                        f.write(chunk)
        except ClientError as err:
            logger.error(
                "Couldn't get image frame. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise

    # snippet-end:[python.example_code.medical-imaging.GetImageFrame]

    # snippet-start:[python.example_code.medical-imaging.DeleteImageSet]
    def delete_image_set(self, datastore_id, image_set_id):
        """
        Delete an image set.

        :param datastore_id: The ID of the data store.
        :param image_set_id: The ID of the image set.
        :return: The delete results.
        """
        try:
            delete_results = self.medical_imaging_client.delete_image_set(
                imageSetId=image_set_id, datastoreId=datastore_id
            )
        except ClientError as err:
            logger.error(
                "Couldn't delete image set. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise
        else:
            return delete_results

        # snippet-end:[python.example_code.medical-imaging.DeleteImageSet]

# snippet-end:[python.example_code.medical-imaging.MedicalImagingWrapper.class]
