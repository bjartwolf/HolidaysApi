FROM mono:latest
EXPOSE 7000
COPY build .
CMD [ "mono", "./HolidaysApi.Server.exe"]
