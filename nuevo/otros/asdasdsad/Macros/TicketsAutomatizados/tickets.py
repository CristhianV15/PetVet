from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
import time
import openpyxl

# Ruta al archivo de Excel
excel_file = 'D:/Macros/TicketsAutomatizados/MacroAutorizacionesSAP.xlsm'

# Cargar el archivo Excel
wb = openpyxl.load_workbook(excel_file)
sheet = wb.active

# Extraer el valor de la celda que contiene el link de la página
link_mda = sheet.cell(row=2, column=7).value  # Suponiendo que el link está en la celda G2

# Configurar el driver de Chrome utilizando el Service
from selenium.webdriver.chrome.service import Service

# Ruta al ChromeDriver (debe ser el path correcto)
chrome_driver_path = "D:/Macros/TicketsAutomatizados/chromedriver.exe"

# Crear un servicio con la ruta del ChromeDriver
service = Service(chrome_driver_path)

# Iniciar el navegador con el servicio configurado
driver = webdriver.Chrome(service=service)

# Navegar a la página web
driver.get(link_mda)

# Esperar a que la página cargue completamente (ajustar tiempo según sea necesario)
time.sleep(5)  # Esto es una espera activa, mejor utilizar WebDriverWait en un caso real

# Rellenar el campo de texto (usando el 'id' del campo)
textarea = driver.find_element(By.ID, "sp_formfield_short_description")
textarea.send_keys("123")  # Enviar el texto "123"

# Opcional: Enviar el formulario (si hay un botón de envío)
# submit_button = driver.find_element(By.ID, "submit_button_id")  # Cambia esto según el ID real del botón
# submit_button.click()

# Cerrar el navegador
driver.quit()




# Opcional: Enviar el formulario (si hay un botón de envío)
# submit_button = driver.find_element(By.ID, "submit_button_id")  # Cambia esto según el ID real del botón
# submit_button.click()

# Cerrar el navegador
# driver.quit()

#from selenium.webdriver.support.ui import WebDriverWait
#from selenium.webdriver.support import expected_conditions as EC

# Esperar hasta que el campo de texto esté disponible
#textarea = WebDriverWait(driver, 10).until(
 #   EC.presence_of_element_located((By.ID, "sp_formfield_short_description"))
#)
#textarea.send_keys("123")
