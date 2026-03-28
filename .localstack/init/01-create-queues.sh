#!/bin/bash

set -euo pipefail

echo "Creating SQS queues..."

awslocal sqs create-queue \
  --queue-name orders \
  --attributes VisibilityTimeout=30,MessageRetentionPeriod=86400

awslocal sqs create-queue \
  --queue-name orders-dlq \
  --attributes VisibilityTimeout=30,MessageRetentionPeriod=1209600

MAIN_QUEUE_URL=$(awslocal sqs get-queue-url --queue-name orders --query QueueUrl --output text)
DLQ_URL=$(awslocal sqs get-queue-url --queue-name orders-dlq --query QueueUrl --output text)
DLQ_ARN=$(awslocal sqs get-queue-attributes --queue-url "$DLQ_URL" --attribute-names QueueArn --query Attributes.QueueArn --output text)

awslocal sqs set-queue-attributes \
  --queue-url "$MAIN_QUEUE_URL" \
  --attributes "{\"RedrivePolicy\":\"{\\\"deadLetterTargetArn\\\":\\\"$DLQ_ARN\\\",\\\"maxReceiveCount\\\":\\\"3\\\"}\"}"

echo "SQS queues created successfully."
echo "  orders:     $MAIN_QUEUE_URL"
echo "  orders-dlq: $DLQ_URL"
