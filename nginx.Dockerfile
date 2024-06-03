FROM nginx

COPY nginx.local.conf /etc/nginx/nginx.conf
COPY id-local.crt /etc/ssl/certs/id-local.eshopping.com.crt
COPY id-local.key /etc/ssl/private/id-local.eshopping.com.key