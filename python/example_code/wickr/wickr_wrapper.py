# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Purpose

Shows how to use the AWS SDK for Python (Boto3) to work with AWS Wickr.
"""

import logging
import time
from typing import Dict, List, Any
import boto3
from botocore.exceptions import ClientError

logger = logging.getLogger(__name__)


class WickrWrapper:
    """Encapsulates AWS Wickr operations."""

    def __init__(self, wickr_client):
        """
        :param wickr_client: A Boto3 Wickr client.
        """
        self.wickr_client = wickr_client

    # snippet-start:[python.example_code.wickr.create_network]
    def create_network(self, network_name: str, access_level: str = 'STANDARD') -> Dict[str, Any]:
        """
        Creates a new Wickr network for secure communications.

        :param network_name: The name of the network to create.
        :param access_level: The access level for the network (STANDARD or ENTERPRISE).
        :return: Response from the create_network operation.
        """
        try:
            response = self.wickr_client.create_network(
                networkName=network_name,
                accessLevel=access_level
            )
            logger.info(f"Created network {network_name} with ID {response['networkId']}")
            return response
        except ClientError as error:
            if error.response['Error']['Code'] == 'NetworkAlreadyExistsException':
                logger.error(f"Network {network_name} already exists")
            elif error.response['Error']['Code'] == 'AccessDeniedException':
                logger.error("Access denied - check permissions")
            elif error.response['Error']['Code'] == 'NetworkQuotaExceededException':
                logger.error("Network quota exceeded")
            else:
                logger.error(f"Error creating network: {error}")
            raise
    # snippet-end:[python.example_code.wickr.create_network]

    # snippet-start:[python.example_code.wickr.list_networks]
    def list_networks(self) -> List[Dict[str, Any]]:
        """
        Lists all available Wickr networks using pagination.

        :return: List of networks.
        """
        try:
            paginator = self.wickr_client.get_paginator('list_networks')
            networks = []
            for page in paginator.paginate():
                networks.extend(page.get('networks', []))
            logger.info(f"Found {len(networks)} network(s)")
            return networks
        except ClientError as error:
            logger.error(f"Error listing networks: {error}")
            raise
    # snippet-end:[python.example_code.wickr.list_networks]

    # snippet-start:[python.example_code.wickr.update_network_settings]
    def update_network_settings(self, network_id: str, encryption_level: str = 'AES256', 
                               message_retention_days: int = 30) -> None:
        """
        Updates network security settings.

        :param network_id: The ID of the network to update.
        :param encryption_level: The encryption level to set.
        :param message_retention_days: Number of days to retain messages.
        """
        try:
            self.wickr_client.update_network_settings(
                networkId=network_id,
                encryptionLevel=encryption_level,
                messageRetentionDays=message_retention_days
            )
            logger.info(f"Updated network settings for {network_id}")
        except ClientError as error:
            if error.response['Error']['Code'] == 'NetworkNotFoundException':
                logger.error(f"Network {network_id} not found")
            else:
                logger.error(f"Error updating network settings: {error}")
            raise
    # snippet-end:[python.example_code.wickr.update_network_settings]

    # snippet-start:[python.example_code.wickr.create_user]
    def create_user(self, network_id: str, username: str, email: str, role: str = 'User') -> Dict[str, Any]:
        """
        Creates a new user in the Wickr network.

        :param network_id: The ID of the network.
        :param username: The username for the new user.
        :param email: The email address for the new user.
        :param role: The role for the new user (Admin or User).
        :return: Response from the create_user operation.
        """
        try:
            response = self.wickr_client.create_user(
                networkId=network_id,
                username=username,
                email=email,
                role=role
            )
            logger.info(f"Created user {username} with ID {response['userId']}")
            return response
        except ClientError as error:
            if error.response['Error']['Code'] == 'UserAlreadyExistsException':
                logger.error(f"User {username} already exists in the network")
            elif error.response['Error']['Code'] == 'NetworkNotFoundException':
                logger.error(f"Network {network_id} not found")
            else:
                logger.error(f"Error creating user: {error}")
            raise
    # snippet-end:[python.example_code.wickr.create_user]

    # snippet-start:[python.example_code.wickr.create_room]
    def create_room(self, network_id: str, room_name: str, description: str, max_members: int = 50) -> Dict[str, Any]:
        """
        Creates a secure messaging room.

        :param network_id: The ID of the network.
        :param room_name: The name of the room.
        :param description: A description of the room.
        :param max_members: Maximum number of members allowed in the room.
        :return: Response from the create_room operation.
        """
        try:
            response = self.wickr_client.create_room(
                networkId=network_id,
                roomName=room_name,
                description=description,
                maxMembers=max_members
            )
            logger.info(f"Created room {room_name} with ID {response['roomId']}")
            return response
        except ClientError as error:
            if error.response['Error']['Code'] == 'NetworkNotFoundException':
                logger.error(f"Network {network_id} not found")
            else:
                logger.error(f"Error creating room: {error}")
            raise
    # snippet-end:[python.example_code.wickr.create_room]

    # snippet-start:[python.example_code.wickr.invite_to_room]
    def invite_to_room(self, room_id: str, user_ids: List[str]) -> None:
        """
        Invites users to a room.

        :param room_id: The ID of the room.
        :param user_ids: List of user IDs to invite.
        """
        try:
            self.wickr_client.invite_to_room(
                roomId=room_id,
                userIds=user_ids
            )
            logger.info(f"Invited {len(user_ids)} user(s) to room {room_id}")
        except ClientError as error:
            if error.response['Error']['Code'] == 'RoomNotFoundException':
                logger.error(f"Room {room_id} not found")
            elif error.response['Error']['Code'] == 'RoomFullException':
                logger.error("Room has reached maximum capacity")
            else:
                logger.error(f"Error inviting users to room: {error}")
            raise
    # snippet-end:[python.example_code.wickr.invite_to_room]

    # snippet-start:[python.example_code.wickr.update_room_permissions]
    def update_room_permissions(self, room_id: str, allow_file_sharing: bool = True, 
                               message_expiration: int = 3600) -> None:
        """
        Updates room permissions and security settings.

        :param room_id: The ID of the room.
        :param allow_file_sharing: Whether to allow file sharing in the room.
        :param message_expiration: Message expiration time in seconds.
        """
        try:
            self.wickr_client.update_room_permissions(
                roomId=room_id,
                allowFileSharing=allow_file_sharing,
                messageExpiration=message_expiration
            )
            logger.info(f"Updated permissions for room {room_id}")
        except ClientError as error:
            if error.response['Error']['Code'] == 'RoomNotFoundException':
                logger.error(f"Room {room_id} not found")
            else:
                logger.error(f"Error updating room permissions: {error}")
            raise
    # snippet-end:[python.example_code.wickr.update_room_permissions]

    # snippet-start:[python.example_code.wickr.send_message]
    def send_message(self, room_id: str, content: str, message_type: str = 'text') -> Dict[str, Any]:
        """
        Sends a message to a room.

        :param room_id: The ID of the room.
        :param content: The message content.
        :param message_type: The type of message (text, file, etc.).
        :return: Response from the send_message operation.
        """
        try:
            response = self.wickr_client.send_message(
                roomId=room_id,
                content=content,
                messageType=message_type
            )
            logger.info(f"Sent message to room {room_id}: {response['messageId']}")
            return response
        except ClientError as error:
            if error.response['Error']['Code'] == 'RoomNotFoundException':
                logger.error(f"Room {room_id} not found")
            elif error.response['Error']['Code'] == 'UserNotInRoomException':
                logger.error("User not authorized for this room")
            elif error.response['Error']['Code'] == 'InvalidMessageTypeException':
                logger.error(f"Invalid message type: {message_type}")
            else:
                logger.error(f"Error sending message: {error}")
            raise
    # snippet-end:[python.example_code.wickr.send_message]

    # snippet-start:[python.example_code.wickr.upload_file]
    def upload_file(self, room_id: str, file_path: str, description: str) -> Dict[str, Any]:
        """
        Uploads a file to a room.

        :param room_id: The ID of the room.
        :param file_path: The path to the file to upload.
        :param description: A description of the file.
        :return: Response from the upload_file operation.
        """
        try:
            response = self.wickr_client.upload_file(
                roomId=room_id,
                filePath=file_path,
                description=description
            )
            logger.info(f"Uploaded file to room {room_id}: {response['fileId']}")
            return response
        except ClientError as error:
            if error.response['Error']['Code'] == 'RoomNotFoundException':
                logger.error(f"Room {room_id} not found")
            elif error.response['Error']['Code'] == 'FileTooLargeException':
                logger.error("File exceeds size limit")
            else:
                logger.error(f"Error uploading file: {error}")
            raise
    # snippet-end:[python.example_code.wickr.upload_file]

    # snippet-start:[python.example_code.wickr.set_message_expiration]
    def set_message_expiration(self, room_id: str, expiration_time: int) -> None:
        """
        Sets message expiration for a room.

        :param room_id: The ID of the room.
        :param expiration_time: Expiration time in seconds.
        """
        try:
            self.wickr_client.set_message_expiration(
                roomId=room_id,
                expirationTime=expiration_time
            )
            logger.info(f"Set message expiration for room {room_id} to {expiration_time} seconds")
        except ClientError as error:
            if error.response['Error']['Code'] == 'RoomNotFoundException':
                logger.error(f"Room {room_id} not found")
            else:
                logger.error(f"Error setting message expiration: {error}")
            raise
    # snippet-end:[python.example_code.wickr.set_message_expiration]

    # snippet-start:[python.example_code.wickr.enable_burn_on_read]
    def enable_burn_on_read(self, room_id: str, enabled: bool = True) -> None:
        """
        Enables or disables burn-on-read for messages in a room.

        :param room_id: The ID of the room.
        :param enabled: Whether to enable burn-on-read.
        """
        try:
            self.wickr_client.enable_burn_on_read(
                roomId=room_id,
                enabled=enabled
            )
            action = "Enabled" if enabled else "Disabled"
            logger.info(f"{action} burn-on-read for room {room_id}")
        except ClientError as error:
            if error.response['Error']['Code'] == 'RoomNotFoundException':
                logger.error(f"Room {room_id} not found")
            else:
                logger.error(f"Error setting burn-on-read: {error}")
            raise
    # snippet-end:[python.example_code.wickr.enable_burn_on_read]

    # snippet-start:[python.example_code.wickr.get_room_members]
    def get_room_members(self, room_id: str) -> List[Dict[str, Any]]:
        """
        Gets the members of a room.

        :param room_id: The ID of the room.
        :return: List of room members.
        """
        try:
            response = self.wickr_client.get_room_members(roomId=room_id)
            members = response.get('members', [])
            logger.info(f"Found {len(members)} member(s) in room {room_id}")
            return members
        except ClientError as error:
            if error.response['Error']['Code'] == 'RoomNotFoundException':
                logger.error(f"Room {room_id} not found")
            else:
                logger.error(f"Error getting room members: {error}")
            raise
    # snippet-end:[python.example_code.wickr.get_room_members]

    # snippet-start:[python.example_code.wickr.get_audit_logs]
    def get_audit_logs(self, network_id: str, start_date: str, end_date: str) -> List[Dict[str, Any]]:
        """
        Retrieves audit logs for a network using pagination.

        :param network_id: The ID of the network.
        :param start_date: Start date for logs (YYYY-MM-DD format).
        :param end_date: End date for logs (YYYY-MM-DD format).
        :return: List of audit log entries.
        """
        try:
            paginator = self.wickr_client.get_paginator('get_audit_logs')
            logs = []
            for page in paginator.paginate(
                networkId=network_id,
                startDate=start_date,
                endDate=end_date
            ):
                logs.extend(page.get('logs', []))
            logger.info(f"Retrieved {len(logs)} audit log entries")
            return logs
        except ClientError as error:
            if error.response['Error']['Code'] == 'NetworkNotFoundException':
                logger.error(f"Network {network_id} not found")
            else:
                logger.error(f"Error retrieving audit logs: {error}")
            raise
    # snippet-end:[python.example_code.wickr.get_audit_logs]

    # snippet-start:[python.example_code.wickr.delete_network]
    def delete_network(self, network_id: str) -> None:
        """
        Deletes a Wickr network.

        :param network_id: The ID of the network to delete.
        """
        try:
            self.wickr_client.delete_network(networkId=network_id)
            logger.info(f"Deleted network {network_id}")
        except ClientError as error:
            if error.response['Error']['Code'] == 'NetworkNotFoundException':
                logger.error(f"Network {network_id} not found")
            else:
                logger.error(f"Error deleting network: {error}")
            raise
    # snippet-end:[python.example_code.wickr.delete_network]

    # snippet-start:[python.example_code.wickr.get_operation_status]
    def get_operation_status(self, operation_id: str) -> str:
        """
        Gets the status of an operation.

        :param operation_id: The ID of the operation.
        :return: The operation status.
        :raises ClientError: If getting the operation status fails.
        """
        try:
            response = self.wickr_client.get_operation_status(
                operationId=operation_id
            )
            return response['status']
        except ClientError as error:
            if error.response['Error']['Code'] == 'OperationNotFoundException':
                logger.error("Operation not found")
            else:
                logger.error(f"Error getting operation status: {error}")
            raise
    # snippet-end:[python.example_code.wickr.get_operation_status]

    # snippet-start:[python.example_code.wickr.wait_for_operation]
    def wait_for_operation(self, operation_id: str, max_wait_time: int = 300) -> str:
        """
        Waits for an operation to complete.

        :param operation_id: The ID of the operation to wait for.
        :param max_wait_time: Maximum time to wait in seconds.
        :return: The final operation status.
        :raises ClientError: If the operation fails or times out.
        """
        start_time = time.time()
        while time.time() - start_time < max_wait_time:
            status = self.get_operation_status(operation_id)
            logger.info(f"Operation {operation_id} status: {status}")
            
            if status in ['SUCCEEDED', 'COMPLETED']:
                return status
            elif status in ['FAILED', 'CANCELLED']:
                raise ClientError(
                    error_response={
                        'Error': {
                            'Code': 'OperationFailed',
                            'Message': f'Operation {operation_id} failed with status: {status}'
                        }
                    },
                    operation_name='WaitForOperation'
                )
            
            time.sleep(10)  # Wait 10 seconds before checking again
        
        raise ClientError(
            error_response={
                'Error': {
                    'Code': 'OperationTimeout',
                    'Message': f'Operation {operation_id} timed out after {max_wait_time} seconds'
                }
            },
            operation_name='WaitForOperation'
        )
    # snippet-end:[python.example_code.wickr.wait_for_operation]
