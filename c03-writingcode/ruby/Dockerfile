FROM ruby:2.7.1-alpine

WORKDIR /app

RUN gem install sinatra

COPY app.rb .

ENTRYPOINT ["ruby"]

CMD ["app.rb"]