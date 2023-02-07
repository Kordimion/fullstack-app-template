FROM quay.io/keycloak/keycloak:latest as builder


WORKDIR /opt/keycloak


#ENV KC_METRICS_ENABLED=true
ENV KC_FEATURES='token-exchange scripts'
ENV KC_DB=postgres
ENV KEYCLOAK_ADMIN=admin
ENV KEYCLOAK_ADMIN_PASSWORD=admin
ENV KC_DB_URL='jdbc:postgresql://keycloakdb/keycloak'
ENV KC_DB_USERNAME=postgres
ENV KC_DB_PASSWORD=postgres
ENV KC_HOSTNAME=localhost
ENV KC_HTTP_RELATIVE_PATH='/identity/'
ENV KC_HOSTNAME_STRICT=false
ENV KC_HTTP_ENABLED=true
#ENV KC_HOSTNAME_PORT=443
ENV PROXY_ADDRESS_FORWARDING=true
ENV KC_PROXY=edge

RUN /opt/keycloak/bin/kc.sh build --db=postgres
ENTRYPOINT /opt/keycloak/bin/kc.sh start --import-realm