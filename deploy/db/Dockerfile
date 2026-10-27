FROM mcr.microsoft.com/mssql/server
ARG MSSQL_PASSWORD=P@ssw0rd
ENV MSSQL_SA_PASSWORD=${MSSQL_PASSWORD}
ENV ACCEPT_EULA=Y

COPY ./*.sql /tmp/sql/

RUN ( /opt/mssql/bin/sqlservr --accept-eula &) | grep -q "Service Broker manager has started."\
    && /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P ${MSSQL_SA_PASSWORD} -d master -i /tmp/sql/01-init.sql\
    && pkill sqlservr 