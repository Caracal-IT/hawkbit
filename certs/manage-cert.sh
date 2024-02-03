#!/bin/bash

ACTION=$1
CERT_PATH="./certs/server.crt"
TRUSTED_CERT_DIR="/usr/local/share/ca-certificates"

if [ "$ACTION" == "add" ]; then
    sudo cp $CERT_PATH $TRUSTED_CERT_DIR
    sudo update-ca-certificates
    echo "Certificate added to the store."
elif [ "$ACTION" == "remove" ]; then
    CERT_NAME=$(basename $CERT_PATH)
    sudo rm $TRUSTED_CERT_DIR/$CERT_NAME
    sudo update-ca-certificates --fresh
    echo "Certificate removed from the store."
else
    echo "Invalid action. Please use 'add' or 'remove'."
fi