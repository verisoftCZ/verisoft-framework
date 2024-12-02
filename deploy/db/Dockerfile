FROM mcr.microsoft.com/mssql/server
ENV SA_PASSWORD=your.password!
ENV ACCEPT_EULA=Y

COPY ./*.sql /tmp/sql/

RUN ( /opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" \
    && /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P ${SA_PASSWORD} -d master -i /tmp/sql/01-init.sql\
    && pkill sqlservr 