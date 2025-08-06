FROM quay.io/keycloak/keycloak:24.0.2

USER root
COPY keycloak-certs/ldap.cer /tmp/ldap.cer
RUN keytool -import -trustcacerts -alias myldap -file /tmp/ldap.cer -keystore /etc/pki/ca-trust/extracted/java/cacerts -storepass changeit -noprompt || true
RUN keytool -import -trustcacerts -alias myldap -file /tmp/ldap.cer -keystore /etc/java/java-17-openjdk/java-17-openjdk-17.0.10.0.7-2.el9.x86_64/lib/security/cacerts -storepass changeit -noprompt || true
RUN keytool -import -trustcacerts -alias myldap -file /tmp/ldap.cer -keystore /pki/java/cacerts -storepass changeit -noprompt || true
USER 1000