# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

# snippet-start:[python.example_code.medical-imaging.workflow]
"""
Purpose

Shows how to use the AWS SDK for Python (Boto3) to manage and invoke AWS HealthImaging
functions.
"""

import datetime
import gzip
import json
import logging
import random
import time
import sys
import os

import boto3
from botocore.exceptions import ClientError
from threading import Thread

# Import the wrapper for the service functionality.
from medicalimaging import MedicalImagingWrapper

# Add relative path to include demo_tools in this code example without need for setup.
sys.path.append("../..")
from demo_tools import demo_func
import demo_tools.question as q

logger = logging.getLogger(__name__)

TEMPLATES_PATH = "templates"

DATASTORE_PARAMETER = "datastoreName"
USER_ACCOUNT_ID_PARAMETER = "userAccountID"
ROLE_ARN_OUTPUT = "RoleArn"
BUCKET_NAME_OUTPUT = "InputBucketName"
OUTPUT_BUCKET_NAME_OUTPUT = "OutputBucketName"
DATASTORE_ID_OUTPUT = "DatastoreID"

IDC_S3_BUCKET_NAME = "idc-open-data"

IDC_IMAGE_CHOICES = [
    {"Description": "CT of chest (2 images)", "Directory": "00029d25-fb18-4d42-aaa5-a0897d1ac8f7"},
    {"Description": "CT of pelvis (57 images)", "Directory": "00025d30-ef8f-4135-a35a-d83eff264fc1"},
    {"Description": "MRI of head (192 images)", "Directory": "0002d261-8a5d-4e63-8e2e-0cbfac87b904"},
    {"Description": "MRI of breast (92 images)", "Directory": "0002dd07-0b7f-4a68-a655-44461ca34096"}
]

IMPORT_JOB_MANIFEST_FILE_NAME = "job-output-manifest.json"


class MedicalImagingWorkflowScenario:
    def __init__(self, medical_imaging_wrapper, s3_client):
        self.medical_imaging_wrapper = medical_imaging_wrapper
        self.s3_client = s3_client

    def run_scenario(self, input_bucket_name, output_bucket_name, role_arn):

        print("-" * 88)
        print("Welcome to the AWS HealthImaging working with image sets and frames workflow.")
        print("-" * 88)

        print("""
        This workflow will import DICOM files into a HealthImaging data store.
        DICOM® — Digital Imaging and Communications in Medicine — is the international
        standard for medical images and related information.
        
        The workflow will then download all the image frames created during the DICOM import and decode
        the image frames from their HTJ2K format to a bitmap format.
        The bitmaps will then be validated with a checksum to ensure they are correct.
        This workflow requires a number of AWS resources to run.
        
        It requires a HealthImaging data store, an Amazon Simple Storage Service (Amazon S3)
        bucket for uploaded DICOM files, an Amazon S3 bucket for the output of a DICOM import, and
        an AWS Identity and Access Management (IAM) role for importing the DICOM files into
        the data store.
        
        These resources can be created separately using the provided AWS CloudFormation stack:
        todo: provide link.
        
        This workflow uses DICOM files from the National Cancer Institute Imaging Data Commons (IDC)
        Collections.
        
        Here is the link to their website:
        https://registry.opendata.aws/nci-imaging-data-commons/
        We will use DICOM files stored in an S3 bucket managed by the IDC.

        First one of the DICOM folders in the IDC collection must be copied to your
        input S3 bucket.
        
        """)

        print(f"You have the choice of one of the following {len(IDC_IMAGE_CHOICES)} folders to copy.")

        for index, idcChoice in enumerate(IDC_IMAGE_CHOICES):
            print(f"\t{index + 1}. {idcChoice.mDescription}")
        choice = q.ask(
            "Which DICOM files do you want to import? ",
            q.is_int,
            q.in_range(1, len(IDC_IMAGE_CHOICES) + 1),
        )

        from_directory = IDC_IMAGE_CHOICES[choice - 1].mDirectory
        input_directory = "input"

        print(f"The files in the directory {from_directory} in the bucket {IDC_S3_BUCKET_NAME} will be copied ")
        print(f"to the folder {input_directory}/{from_directory}in the bucket {input_bucket_name}.")
        q.ask("Press Enter to start the copy.")
        self.copy_images(from_directory, input_directory)

        print(f"\nNow the DICOM images will be imported into the datastore with ID {data_store_id}.")
        import_job_id = self.medical_imaging_wrapper.start_dicom_import_job(
            data_store_id, input_bucket_name, input_directory, output_bucket_name, input_directory, role_arn)
        print(f"\nThe DICOM files were successfully imported. The import job ID is {data_store_id}.")

        print(f"""
        Information about the import job, including the IDs of the created image sets,
        is located in a file named {IMPORT_JOB_MANIFEST_FILE_NAME} 
        This file is located in a folder specified by the import job's 'outputS3Uri'.
        The 'outputS3Uri' is retrieved by calling the 'GetDICOMImportJob' action.
        """)
        print("-" * 88)
        print(f"""
        The image set IDs will be retrieved by downloading '{IMPORT_JOB_MANIFEST_FILE_NAME}' 
        file from the output S3 bucket.
        """)
        q.ask("Press Enter to continue.")

        image_sets = self.medical_imaging_wrapper.get_image_frames_for_image_set(data_store_id, import_job_id)

        print("The image sets created by this import job are:")
        for image_set in image_sets:
            print("Image set:", image_set)

        print("""
        If you would like information about how HealthImaging organizes image sets,
        go to the following link.
        https://docs.aws.amazon.com/healthimaging/latest/devguide/understanding-image-sets.html
        """)

        q.ask("Press Enter to continue.")
        print("-" * 88)

        print("""
        Next this workflow will download all the image frames created in this import job. 
        The IDs of all the image frames in an image set are stored in the image set metadata.
        The image set metadata will be downloaded and parsed for the image frame IDs.
        """)

        q.ask("Press Enter to continue.")

        out_dir = f"output/import_job_{import_job_id}"
        os.makedirs(out_dir, exist_ok=True)

        all_image_frame_ids = []
        for image_set in image_sets:
            image_frames = self.medical_imaging_wrapper.get_image_frames_for_image_set(data_store_id, image_set, out_dir)

            all_image_frame_ids.extend(image_frames)

        print(f"{all_image_frame_ids.len()} image frames were created by this import job.")
        print("-" * 88)

        print("""
        The image frames are encoded in the HTJ2K format. This example will convert
        the image frames to bitmaps. The decoded images will be verified using 
        a CRC32 checksum retrieved from the image set metadata.
        The OpenJPEG open-source library will be used for the conversion.  
        The following link contains information about HTJ2K decoding libraries.
        https://docs.aws.amazon.com/healthimaging/latest/devguide/reference-htj2k.html
        """)

        q.ask("Press Enter to download and convert the images.")

        result = self.medical_imaging_wrapper.download_decode_and_check_image_frames(
            data_store_id, all_image_frame_ids, out_dir)

        if result:
            print(f"""
                   The image files were successfully decoded and validated.
                   The HTJ2K image files are located in the directory
                   {out_dir} in the working directory
                   The OpenJPEG open-source library will be used for the conversion.  
                   The following link contains information about HTJ2K decoding libraries.
                   https://docs.aws.amazon.com/healthimaging/latest/devguide/reference-htj2k.html
                   """)

        print("-" * 88)
        print("This concludes this workflow.")
        # TODO: Delete image frames?
        self.cleanup()
        print("\nThanks for watching!")
        print("-" * 88)

    def cleanup(self):
        """
        1. Cleans up files created by the workflow.
        """
        if q.ask(
            f"Clean up files created by the workflow? (y/n) ",
            q.is_yesno,
        ):
            # TODO: clean everything up.
            print("Removed files created by the workflow.")

    def copy_single_object(self, key, source_bucket, target_bucket):
        """
        Copies a single object from a source to a target bucket.

        :param key: The key of the object to copy.
        :param source_bucket: The source bucket to copy from.
        :param target_bucket: The target bucket to copy to.
        """
        copy_source = {
            'Bucket': source_bucket,
            'Key': key
        }
        self.s3_client.copy_object(CopySource=copy_source, Bucket=target_bucket, Key=key)
        print(f"\tCopying {key}.")

    def copy_images(self, source_bucket, target_bucket):
        """
        Copies the images from the source to the target bucket using multiple threads.

        :param source_bucket: The source bucket for the images.
        :param target_bucket: The target bucket for the images.
        """

        # Get list of all objects in source bucket.
        objs = self.s3_client.list_objects_v2(Bucket=source_bucket)['Contents']
        keys = [obj['Key'] for obj in objs]

        # Copy the objects with multiple threads.
        threads = []
        for key in keys:
            t = Thread(target=self.copy_single_object, args=[key, source_bucket, target_bucket])
            threads.append(t)
            t.start()

        for t in threads:
            t.join()

        print("\tDone copying all objects.")


if __name__ == "__main__":

    # Replace these values with your own, after deploying the AWS Cloud Formation resource stack.
    source_s3_uri = "s3://medical-imaging-dicom-input/dicom_input/"
    dest_s3_uri = "s3://medical-imaging-output/job_output/"
    data_access_role_arn = "arn:aws:iam::565846806325:role/healthimagingworkflow-docexampleimportrole77AD6B59-dkOpwZO4GH8o"
    data_store_id = "e964880c2ca04f51843997e9d4b24ed1"
    bucket_name = "healthimagingworkflow-docexampledicombucket120080d-yrmpiyrzv5pi"
    input_bucket = "medical-imaging-dicom-input"
    output_bucket = "medical-imaging-output"

    try:
        s3 = boto3.client('s3')
        scenario = MedicalImagingWorkflowScenario(MedicalImagingWrapper.from_client(), s3)
        scenario.run_scenario(input_bucket, output_bucket, data_access_role_arn, data_store_id)
    except Exception:
        logging.exception("Something went wrong with the workflow.")
# snippet-end:[python.example_code.medical-imaging.workflow]
