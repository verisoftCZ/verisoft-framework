FROM node:lts AS build
ARG APP_VERSION=1.0.0

WORKDIR /app
ENV NX_DAEMON=false
COPY ./package.json ./package-lock.json /app/
ENV NODE_OPTIONS --max_old_space_size=10240
RUN npm set progress=false && npm install
COPY . .
RUN npm run build -- --prod
RUN sed -Ei -e "s/__APP_VERSION__/${APP_VERSION}/" /app/dist/app/browser/main*.js

FROM nginx:stable-alpine
COPY --from=build /app/dist/app/browser /usr/share/nginx/html
COPY ./deploy/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
