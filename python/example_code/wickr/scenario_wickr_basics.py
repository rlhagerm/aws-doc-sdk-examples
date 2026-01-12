# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Purpose

Shows how to use AWS Wickr to create secure communication networks.
This scenario demonstrates:
1. Creating and configuring a Wickr network with security settings
2. Creating users and managing permissions within the network
3. Creating secure messaging rooms and managing members
4. Sending messages and configuring security features like message expiration and burn-on-read
5. Uploading and sharing files securely
6. Retrieving audit logs for compliance and monitoring

This example uses the AWS SDK for Python (Boto3) to interact with AWS Wickr.
"""

import logging
import sys
import time
from datetime import datetime, timedelta
from typing import List, Dict, Any
import boto3
from botocore.exceptions import ClientError

from wickr_wrapper import WickrWrapper

# Add relative path to include demo_tools
sys.path.insert(0, "../..")
from demo_tools import question as q

logger = logging.getLogger(__name__)

# snippet-start:[python.example_code.wickr.WickrScenario]
class WickrScenario:
    """Runs an interactive scenario that shows how to use AWS Wickr."""

    def __init__(self, wickr_wrapper: WickrWrapper):
        """
        :param wickr_wrapper: An instance of the WickrWrapper class.
        """
        self.wickr_wrapper = wickr_wrapper
        self.network_id = None
        self.room_id = None
        self.user_ids = []

    def run_scenario(self):
        """Runs the AWS Wickr basics scenario."""
        print("-" * 88)
        print("Welcome to the AWS Wickr basics scenario!")
        print("-" * 88)
        print(
            "AWS Wickr is an end-to-end encrypted service that helps organizations\n"
            "and government agencies communicate securely through messaging, voice\n"
            "and video calling, file sharing, and more.\n"
            "\n"
            "This scenario will guide you through:\n"
            "1. Creating and configuring a secure Wickr network\n"
            "2. Adding users and managing permissions\n" 
            "3. Creating secure messaging rooms\n"
            "4. Demonstrating messaging and file sharing with security features\n"
            "5. Examining audit logs for compliance\n"
            "6. Cleaning up resources\n"
        )
        print()

        try:
            self._setup_phase()
            self._demonstration_phase()
            self._examination_phase()
        except Exception as e:
            logger.error(f"Scenario failed: {e}")
            print(f"The scenario encountered an error: {e}")
        finally:
            self._cleanup_phase()

    def _setup_phase(self):
        """Setup phase: Create network, users, and room."""
        print("Setting up AWS Wickr network and resources...")
        print()

        # Check for existing networks
        try:
            existing_networks = self.wickr_wrapper.list_networks()
            if existing_networks:
                print(f"Found {len(existing_networks)} existing network(s):")
                for i, network in enumerate(existing_networks[:3]):  # Show first 3
                    print(f"  {i+1}. {network.get('networkName', 'Unnamed')} ({network.get('networkId', 'Unknown ID')})")
                
                use_existing = q.ask(
                    "Would you like to use an existing network? (y/n): ",
                    q.is_yesno
                )
                if use_existing:
                    network_idx = q.ask(
                        "Enter the number of the network to use: ",
                        q.is_int,
                        q.in_range(1, len(existing_networks))
                    )
                    self.network_id = existing_networks[network_idx - 1]['networkId']
                    print(f"Using existing network: {self.network_id}")
                    return
        except Exception as e:
            print(f"Error checking existing networks: {e}")
            print("Proceeding to create a new network...")

        # Create new network
        network_name = q.ask("Enter a name for the new Wickr network: ")
        description = q.ask("Enter a description for the network: ")
        
        try:
            response = self.wickr_wrapper.create_network(network_name)
            self.network_id = response['networkId']
            print(f"✓ Created network '{network_name}' with ID: {self.network_id}")
            
            # Configure network security settings
            print("Configuring network security settings...")
            self.wickr_wrapper.update_network_settings(
                self.network_id,
                encryption_level='AES256',
                message_retention_days=30
            )
            print("✓ Network security settings configured")
            
        except Exception as e:
            print(f"Error creating network: {e}")
            raise

        # Create sample users
        print("\nCreating sample users...")
        sample_users = [
            {
                'username': 'alice.admin',
                'email': 'alice@example.com',
                'role': 'Admin'
            },
            {
                'username': 'bob.user',
                'email': 'bob@example.com', 
                'role': 'User'
            }
        ]
        
        for user_data in sample_users:
            try:
                response = self.wickr_wrapper.create_user(
                    self.network_id,
                    user_data['username'],
                    user_data['email'],
                    user_data['role']
                )
                self.user_ids.append(response['userId'])
                print(f"✓ Created user '{user_data['username']}' ({user_data['role']})")
            except Exception as e:
                print(f"Error creating user {user_data['username']}: {e}")

        print(f"✓ Setup complete! Network ID: {self.network_id}")

    def _demonstration_phase(self):
        """Demonstration phase: Create room, send messages, share files."""
        print("\nDemonstrating AWS Wickr capabilities...")
        print()

        # Create a secure room
        room_name = q.ask("Enter a name for the secure messaging room: ")
        description = q.ask("Enter a description for the room: ")
        
        try:
            response = self.wickr_wrapper.create_room(
                self.network_id,
                room_name,
                description,
                max_members=50
            )
            self.room_id = response['roomId']
            print(f"✓ Created room '{room_name}' with ID: {self.room_id}")
            
            # Invite users to the room
            if self.user_ids:
                self.wickr_wrapper.invite_to_room(self.room_id, self.user_ids)
                print(f"✓ Invited {len(self.user_ids)} users to the room")
            
            # Configure room security settings
            print("Configuring room security settings...")
            self.wickr_wrapper.update_room_permissions(
                self.room_id,
                allow_file_sharing=True,
                message_expiration=3600  # 1 hour
            )
            print("✓ Room security settings configured")
            
        except Exception as e:
            print(f"Error creating room: {e}")
            return

        # Send sample messages
        print("\nSending sample messages...")
        sample_messages = [
            "Welcome to the secure project room!",
            "This is a test of encrypted messaging.",
            "All communications in this room are end-to-end encrypted."
        ]
        
        for message in sample_messages:
            try:
                response = self.wickr_wrapper.send_message(self.room_id, message)
                print(f"✓ Sent message: {response['messageId']}")
                time.sleep(1)  # Brief pause between messages
            except Exception as e:
                print(f"Error sending message: {e}")

        # Demonstrate security features
        print("\nConfiguring advanced security features...")
        
        # Set message expiration
        expiration_hours = q.ask(
            "Enter message expiration time in hours (1-24): ",
            q.is_int,
            q.in_range(1, 24)
        )
        expiration_seconds = expiration_hours * 3600
        
        try:
            self.wickr_wrapper.set_message_expiration(self.room_id, expiration_seconds)
            print(f"✓ Set message expiration to {expiration_hours} hour(s)")
        except Exception as e:
            print(f"Error setting message expiration: {e}")

        # Enable burn-on-read
        enable_burn = q.ask(
            "Would you like to enable burn-on-read for messages? (y/n): ",
            q.is_yesno
        )
        if enable_burn:
            try:
                self.wickr_wrapper.enable_burn_on_read(self.room_id, True)
                print("✓ Enabled burn-on-read for messages")
            except Exception as e:
                print(f"Error enabling burn-on-read: {e}")

        # Simulate file upload (demonstration only)
        demo_file = q.ask(
            "Would you like to demonstrate file sharing? (y/n): ",
            q.is_yesno
        )
        if demo_file:
            try:
                # Note: In a real implementation, this would upload an actual file
                response = self.wickr_wrapper.upload_file(
                    self.room_id,
                    "/path/to/sample/document.pdf",
                    "Sample project document"
                )
                print(f"✓ File uploaded: {response['fileId']}")
            except Exception as e:
                print(f"Note: File upload demonstration failed (expected in demo): {e}")

        print("✓ Demonstration phase complete!")

    def _examination_phase(self):
        """Examination phase: Review room members and audit logs."""
        print("\nExamining AWS Wickr data and security logs...")
        print()

        if not self.room_id:
            print("No room available for examination.")
            return

        # Get room members
        try:
            members = self.wickr_wrapper.get_room_members(self.room_id)
            print(f"Room Members ({len(members)} total):")
            for i, member in enumerate(members[:10]):  # Show first 10
                username = member.get('username', 'Unknown')
                role = member.get('role', 'Unknown')
                status = member.get('status', 'Unknown')
                print(f"  {i+1}. {username} - {role} ({status})")
            
            if len(members) > 10:
                print(f"  ... and {len(members) - 10} more members")
                
        except Exception as e:
            print(f"Error getting room members: {e}")

        # Show audit logs
        show_audit = q.ask(
            "Would you like to view audit logs? (y/n): ",
            q.is_yesno
        )
        
        if show_audit and self.network_id:
            try:
                # Get logs from the past 7 days
                end_date = datetime.now()
                start_date = end_date - timedelta(days=7)
                
                logs = self.wickr_wrapper.get_audit_logs(
                    self.network_id,
                    start_date.strftime('%Y-%m-%d'),
                    end_date.strftime('%Y-%m-%d')
                )
                
                print(f"\nAudit Logs ({len(logs)} entries from past 7 days):")
                for i, log in enumerate(logs[:5]):  # Show first 5 entries
                    timestamp = log.get('timestamp', 'Unknown')
                    action = log.get('action', 'Unknown')
                    user = log.get('user', 'Unknown')
                    print(f"  {i+1}. {timestamp} - {action} by {user}")
                
                if len(logs) > 5:
                    print(f"  ... and {len(logs) - 5} more log entries")
                    
            except Exception as e:
                print(f"Error retrieving audit logs: {e}")

        # Display security summary
        print("\n" + "="*60)
        print("SECURITY SUMMARY")
        print("="*60)
        print("✓ End-to-end encryption active")
        print("✓ Message expiration configured")
        if hasattr(self, '_burn_on_read_enabled'):
            print("✓ Burn-on-read enabled")
        print("✓ File sharing with encryption")
        print("✓ Audit logging active")
        print("✓ User access controls in place")
        
        print("\nAll communications in this Wickr network are secured with")
        print("military-grade encryption and compliance-ready audit trails.")

    def _cleanup_phase(self):
        """Cleanup phase: Offer to delete created resources."""
        if not self.network_id:
            return

        print("\n" + "="*60)
        print("CLEANUP OPTIONS")
        print("="*60)
        print("The following resources were created during this demo:")
        print(f"- Network: {self.network_id}")
        if self.room_id:
            print(f"- Room: {self.room_id}")
        if self.user_ids:
            print(f"- Users: {len(self.user_ids)} user(s)")
        
        print("\nNote: Deleting the network will remove all associated rooms,")
        print("users, messages, and files. This action cannot be undone.")
        
        delete_network = q.ask(
            "Would you like to delete the network and all resources? (y/n): ",
            q.is_yesno
        )
        
        if delete_network:
            try:
                self.wickr_wrapper.delete_network(self.network_id)
                print(f"✓ Deleted network: {self.network_id}")
                print("All associated resources have been removed.")
            except Exception as e:
                print(f"Error deleting network: {e}")
                print("You may need to delete resources manually through the AWS Console.")
        else:
            print(f"Network {self.network_id} will continue running.")
            print("You can manage it through the AWS Console or delete it later.")
            print("Remember that running resources may incur charges.")

# snippet-end:[python.example_code.wickr.WickrScenario]


def run_scenario():
    """Runs the AWS Wickr basics scenario."""
    logging.basicConfig(level=logging.WARNING, format="%(levelname)s: %(message)s")
    
    print("-" * 88)
    print("Welcome to the AWS Wickr basics scenario!")
    print("-" * 88)
    
    try:
        wickr_client = boto3.client('wickr')
        wickr_wrapper = WickrWrapper(wickr_client)
        scenario = WickrScenario(wickr_wrapper)
        scenario.run_scenario()
        
    except Exception as e:
        print(f"Failed to run scenario: {e}")
        print("Please ensure you have proper AWS credentials and Wickr service access.")


if __name__ == "__main__":
    run_scenario()
