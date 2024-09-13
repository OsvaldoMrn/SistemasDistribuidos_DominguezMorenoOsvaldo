# Etapa 1: Build
FROM golang:1.22 AS build

# Establece el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copia los archivos del proyecto
COPY go.mod ./
COPY main.go ./

# Compila la aplicación
RUN go build -v -o holamundo main.go

# Etapa 2: Final
FROM debian:latest

# Crea un directorio para la aplicación
WORKDIR /app

# Copia el binario desde la etapa de build
COPY --from=build /app/holamundo .

# Exponer el puerto 80
EXPOSE 80

# Comando para ejecutar la aplicación
ENTRYPOINT ["./holamundo"]
