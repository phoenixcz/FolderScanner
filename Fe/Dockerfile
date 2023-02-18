FROM node:16.14.0 AS build
ARG mode=production
ARG VITE_API_URL
ENV VITE_API_URL $VITE_API_URL
ARG VITE_DEFAULT_FOLDER_PATH
ENV VITE_DEFAULT_FOLDER_PATH $VITE_DEFAULT_FOLDER_PATH

RUN npm i --global pnpm

# Create app directory
WORKDIR /app

# A wildcard is used to ensure both package.json AND lock file are copied
# where available (npm@5+)
COPY package.json pnpm-lock.yaml ./

# Install app dependencies
RUN pnpm install --ignore-scripts --frozen-lockfile

# Bundle app source
COPY . .
RUN pnpm build --mode $mode

# Production image
FROM nginx:1.23.2-alpine

RUN addgroup -S appgroup -g 1000 && adduser -S appuser -u 1000 -G appgroup -D -H
USER 1000

COPY --chown=1000 nginx.conf /etc/nginx/nginx.conf
COPY --chown=1000 nginxdefault.conf /etc/nginx/conf.d/default.conf
COPY --chown=1000 --from=build /app/dist/. /usr/share/nginx/html/.

EXPOSE 8080

CMD ["/bin/sh", "-c", "exec nginx -g 'daemon off;'"]
