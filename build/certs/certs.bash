#!/bin/bash

# Define where to store the generated certificates and server key
CERT_DIR="./certs"
mkdir -p $CERT_DIR

# Define certificate details
SUBJECT="/C=US/ST=Denial/L=Springfield/O=Dis/CN=www.example.com"

# Generate a new private key if one doesn't exist, or use the existing one
if [ ! -f $CERT_DIR/server.key ]; then
  openssl genrsa -out $CERT_DIR/server.key 2048
fi

# Generate a CSR using the private key
openssl req -new -key $CERT_DIR/server.key -out $CERT_DIR/server.csr -subj "$SUBJECT"

# Generate a self-signed certificate using the CSR and the private key
openssl x509 -req -days 365 -in $CERT_DIR/server.csr -signkey $CERT_DIR/server.key -out $CERT_DIR/server.crt

# Remove the CSR, we don't need it anymore
rm $CERT_DIR/server.csr

# Generate a PEM file from the private key and the certificate
openssl pkcs12 -export -in $CERT_DIR/server.crt -inkey $CERT_DIR/server.key -out $CERT_DIR/server.p12 -name "server" -passout pass:changeit

# Generate a JKS file from the PEM file
keytool -importkeystore -deststorepass changeit -destkeypass changeit -destkeystore $CERT_DIR/server.jks -srckeystore $CERT_DIR/server.p12 -srcstoretype PKCS12 -srcstorepass changeit -alias server

echo "Certificates generated in $CERT_DIR."