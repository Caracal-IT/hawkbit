#!/bin/bash

# Define where to store the generated certificates and server key
CERT_DIR="./certs"
mkdir -p $CERT_DIR

# Define certificate details
SUBJECT="/C=US/ST=Denial/L=Springfield/O=Dis/CN=www.example.com"

# Define certificate name and password
CERT_NAME="certname"
CERT_PASSWORD="changeit"

# Define the domain names and addresses
DOMAINS=("example.com" "www.example.com")
ADDRESSES=("192.168.0.1" "192.168.0.2")

# Generate a new private key if one doesn't exist, or use the existing one
if [ ! -f $CERT_DIR/$CERT_NAME.key ]; then
  openssl genrsa -out $CERT_DIR/$CERT_NAME.key 2048
fi

# Generate a CSR using the private key
openssl req -new -key $CERT_DIR/$CERT_NAME.key -out $CERT_DIR/$CERT_NAME.csr -subj "$SUBJECT"

# Generate a subject alternative name (SAN) configuration
SAN_CONFIG=""
for ((i=0; i<${#DOMAINS[@]}; i++)); do
  SAN_CONFIG+="DNS:${DOMAINS[i]},IP:${ADDRESSES[i]}"
  if [ $i -lt $((${#DOMAINS[@]}-1)) ]; then
    SAN_CONFIG+=","
  fi
done

# Generate a self-signed certificate using the CSR and the private key
openssl x509 -req -days 365 -in $CERT_DIR/$CERT_NAME.csr -signkey $CERT_DIR/$CERT_NAME.key -out $CERT_DIR/$CERT_NAME.crt -extfile <(printf "subjectAltName=${SAN_CONFIG}")

# Remove the CSR, we don't need it anymore
rm $CERT_DIR/$CERT_NAME.csr

# Generate a PEM file from the private key and the certificate
openssl pkcs12 -export -in $CERT_DIR/$CERT_NAME.crt -inkey $CERT_DIR/$CERT_NAME.key -out $CERT_DIR/$CERT_NAME.p12 -name "$CERT_NAME" -passout pass:$CERT_PASSWORD

# Generate a JKS file from the PEM file
keytool -importkeystore -deststorepass $CERT_PASSWORD -destkeypass $CERT_PASSWORD -destkeystore $CERT_DIR/$CERT_NAME.jks -srckeystore $CERT_DIR/$CERT_NAME.p12 -srcstoretype PKCS12 -srcstorepass $CERT_PASSWORD -alias "$CERT_NAME"

echo "Certificates generated in $CERT_DIR."