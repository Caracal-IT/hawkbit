FROM ubuntu:latest

WORKDIR /app

COPY certs.bash /app

RUN apt-get update && \
    apt-get install -y openssl openjdk-11-jdk-headless && \
    chmod +x /app/certs.bash

CMD ["/app/certs.bash"]