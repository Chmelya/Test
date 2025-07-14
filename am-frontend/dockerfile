FROM node:23-alpine AS build

ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL

WORKDIR /app

COPY package*.json ./
RUN --mount=type=cache,target=/app/node_modules \
    npm install && \
    cp -R node_modules /tmp/node_modules

COPY . .
RUN --mount=type=cache,target=/app/node_modules,id=npm \
    npm run build

FROM node:23-alpine AS final
WORKDIR /app

COPY --from=build /app/package*.json ./
COPY --from=build /app/build ./build
COPY --from=build /tmp/node_modules ./node_modules

EXPOSE 3000
CMD [ "npm", "run", "start" ]
   