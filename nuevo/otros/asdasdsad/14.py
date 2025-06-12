#Commit 01

import turtle
import time
import math

# Configuración inicial de la ventana
window = turtle.Screen()
window.bgcolor("white")
window.title("Feliz 14 Liz")

# Definir la tortuga para dibujar las margaritas
t = turtle.Turtle()
t.speed(0)
t.shape("turtle")
t.width(2)

# Función para dibujar una margarita
def dibujar_margarita(t, tamaño):
    for _ in range(12):  # Se dibujan 12 pétalos
        t.color("yellow")
        t.begin_fill()
        t.circle(tamaño, 60)  # Pétalo
        t.left(120)
        t.circle(tamaño, 60)  # Pétalo
        t.left(60)
        t.end_fill()
        t.left(30)

# Función para dibujar el centro de la margarita
def centro_margarita(t, tamaño):
    t.penup()
    t.goto(0, -tamaño//3)
    t.pendown()
    t.color("orange")
    t.begin_fill()
    t.circle(tamaño//3)
    t.end_fill()

# Función para animar la flor
def animar_margaritas(t):
    for _ in range(6):  # Hacemos un ciclo de 6 margaritas
        t.penup()
        t.goto(math.cos(math.radians(60 * _)) * 200, math.sin(math.radians(60 * _)) * 200)  # Ubicación circular
        t.pendown()
        dibujar_margarita(t, 20)
        centro_margarita(t, 20)

# Función para mostrar el mensaje "Feliz 14 Liz"
def mostrar_mensaje():
    t.penup()
    t.goto(0, -250)
    t.pendown()
    t.color("red")
    t.write("¡Feliz 14 Liz!", align="center", font=("Arial", 24, "bold"))

# Animación final
def animacion_final():
    t.penup()
    t.goto(0, 0)
    t.pendown()
    t.color("purple")
    for _ in range(10):
        t.forward(10)
        t.left(18)
        time.sleep(0.05)
    
# Ejecutar las animaciones
def animar():
    animar_margaritas(t)
    mostrar_mensaje()
    animacion_final()
    
    time.sleep(3)
    window.bye()  # Cierra la ventana después de un tiempo

# Ejecutamos la animación
animar()

# Mantener la ventana abierta
turtle.mainloop()
