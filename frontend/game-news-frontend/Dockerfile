FROM node:19 AS build

WORKDIR /app

COPY package.json ./
COPY package-lock.json ./
RUN npm install
COPY . ./
CMD npm run dev

EXPOSE 5173
RUN npm run build


FROM nginx:1.19-alpine
COPY --from=build /app/build/ /usr/share/nginx/html

EXPOSE 80