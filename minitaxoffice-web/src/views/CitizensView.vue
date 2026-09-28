<script setup>

// ======================================================
// 1. IMPORTACIONES
// ======================================================

/*
  ref:
  crea variables reactivas.

  onMounted:
  ejecuta código cuando aparece este componente.
*/
import { ref, onMounted } from 'vue'


// ======================================================
// 2. VARIABLES
// ======================================================

/*
  Aquí guardaremos los ciudadanos que vienen
  desde nuestra API ASP.NET.
*/
const citizens = ref([])


/*
  Controla si mostramos el formulario.

  false = cerrado
  true  = abierto
*/
const showForm = ref(false)


/*
  Aquí guardamos los datos escritos
  en el formulario.

  Lo utilizaremos tanto para CREAR
  como para EDITAR.
*/
const newCitizen = ref({

  name: '',
  dni: '',
  email: '',
  city: ''

})


/*
  Guarda el ID del ciudadano que estamos editando.

  null:
  NO estamos editando.

  Ejemplo:

  editingId = 2

  significa que estamos editando
  el ciudadano con ID 2.
*/
const editingId = ref(null)


// ======================================================
// 3. URL DE LA API
// ======================================================

const API_URL = 'https://localhost:7278/api/Citizens'


// ======================================================
// 4. GET - CARGAR CIUDADANOS
// ======================================================

/*
  Esta función llama a:

  GET /api/Citizens

  ASP.NET devuelve todos los ciudadanos.
*/
async function loadCitizens() {

  try {

    const response = await fetch(API_URL)


    // Si ASP.NET devuelve un error.
    if (!response.ok) {

      throw new Error(
        'No se pudieron cargar los ciudadanos'
      )

    }


    // Convertimos el JSON recibido
    // en un objeto JavaScript.
    const data = await response.json()


    // Guardamos los ciudadanos.
    //
    // Como citizens es reactivo,
    // Vue actualiza automáticamente la tabla.
    citizens.value = data

  }
  catch (error) {

    console.error(
      'Error cargando ciudadanos:',
      error
    )

  }

}


// ======================================================
// 5. ABRIR FORMULARIO PARA CREAR
// ======================================================

function openForm() {

  /*
    IMPORTANTE:

    Si pulsamos "+ Nuevo ciudadano"
    debemos asegurarnos de que NO estamos
    en modo edición.
  */
  editingId.value = null


  // Limpiamos el formulario.
  newCitizen.value = {

    name: '',
    dni: '',
    email: '',
    city: ''

  }


  // Mostramos el formulario.
  showForm.value = true

}


// ======================================================
// 6. CERRAR FORMULARIO
// ======================================================

function closeForm() {

  // Ocultamos el formulario.
  showForm.value = false


  // Dejamos de estar en modo edición.
  editingId.value = null


  // Limpiamos los campos.
  newCitizen.value = {

    name: '',
    dni: '',
    email: '',
    city: ''

  }

}


// ======================================================
// 7. POST - CREAR CIUDADANO
// ======================================================

/*
  Esta función realiza:

  POST /api/Citizens

  Sirve SOLAMENTE para crear.
*/
async function createCitizen() {

  try {

    const response = await fetch(
      API_URL,
      {

        // POST = crear.
        method: 'POST',


        // Enviamos JSON.
        headers: {

          'Content-Type': 'application/json'

        },


        /*
          Convertimos:

          {
            name: "Samira",
            dni: "...",
            ...
          }

          a JSON.
        */
        body: JSON.stringify(newCitizen.value)

      }
    )


    if (!response.ok) {

      throw new Error(
        'No se pudo crear el ciudadano'
      )

    }


    // Volvemos a cargar la tabla.
    await loadCitizens()


    // Cerramos y limpiamos.
    closeForm()

  }
  catch (error) {

    console.error(
      'Error creando ciudadano:',
      error
    )

  }

}


// ======================================================
// 8. PREPARAR EDICIÓN
// ======================================================

/*
  Esta función NO modifica todavía
  el ciudadano.

  Simplemente:

  1. Guarda su ID.
  2. Abre el formulario.
  3. Copia sus datos dentro de los inputs.
*/
function editCitizen(citizen) {

  // Ejemplo:
  //
  // Rama tiene ID 2.
  //
  // editingId = 2
  editingId.value = citizen.id


  // Copiamos los datos actuales.
  newCitizen.value = {

    name: citizen.name,

    dni: citizen.dni,

    email: citizen.email,

    city: citizen.city

  }


  // Abrimos el formulario.
  showForm.value = true

}


// ======================================================
// 9. PUT - ACTUALIZAR CIUDADANO
// ======================================================

/*
  Esta función realiza:

  PUT /api/Citizens/{id}

  Ejemplo:

  PUT /api/Citizens/2
*/
async function updateCitizen() {

  try {

    /*
      Construimos la URL.

      Si:

      editingId = 2

      obtenemos:

      https://localhost:7278/api/Citizens/2
    */
    const url = `${API_URL}/${editingId.value}`


    const response = await fetch(
      url,
      {

        // PUT = modificar.
        method: 'PUT',


        headers: {

          'Content-Type': 'application/json'

        },


        // Mandamos los nuevos datos.
        body: JSON.stringify(newCitizen.value)

      }
    )


    // Si ASP.NET devuelve error.
    if (!response.ok) {

      throw new Error(
        'No se pudo actualizar el ciudadano'
      )

    }


    // Volvemos a pedir los ciudadanos.
    await loadCitizens()


    // Cerramos el formulario.
    closeForm()

  }
  catch (error) {

    console.error(
      'Error actualizando ciudadano:',
      error
    )

  }

}


// ======================================================
// 10. GUARDAR
// ======================================================

/*
  ESTA FUNCIÓN ES MUY IMPORTANTE.

  El formulario utilizará siempre:

  saveCitizen()

  Y esta función decidirá:

  ¿Estamos creando?

  o

  ¿Estamos editando?
*/
async function saveCitizen() {

  // ============================================
  // CREAR
  // ============================================

  /*
    Si editingId es null:

    significa que hemos pulsado:

    + Nuevo ciudadano

    Por tanto hacemos POST.
  */
  if (editingId.value === null) {

    await createCitizen()

  }


  // ============================================
  // EDITAR
  // ============================================

  /*
    Si editingId tiene un número:

    Ejemplo:

    editingId = 2

    significa que estamos editando
    el ciudadano 2.

    Por tanto hacemos PUT.
  */
  else {

    await updateCitizen()

  }

}


// ======================================================
// 11. DELETE - ELIMINAR CIUDADANO
// ======================================================

/*
  DELETE /api/Citizens/{id}

  Ejemplo:

  DELETE /api/Citizens/2
*/
async function deleteCitizen(id) {

  // Preguntamos antes de eliminar.
  const confirmed = confirm(
    '¿Seguro que quieres eliminar este ciudadano?'
  )


  // Si pulsa Cancelar:
  if (!confirmed) {

    return

  }


  try {

    const response = await fetch(
      `${API_URL}/${id}`,
      {

        // DELETE = eliminar.
        method: 'DELETE'

      }
    )


    if (!response.ok) {

      throw new Error(
        'No se pudo eliminar el ciudadano'
      )

    }


    // Recargamos la tabla.
    await loadCitizens()

  }
  catch (error) {

    console.error(
      'Error eliminando ciudadano:',
      error
    )

  }

}


// ======================================================
// 12. CUANDO APARECE EL COMPONENTE
// ======================================================

/*
  Cuando entramos en la pantalla Ciudadanos:

  CitizensView.vue
        ↓
  onMounted()
        ↓
  loadCitizens()
        ↓
  GET ASP.NET
        ↓
  citizens
        ↓
  tabla
*/
onMounted(() => {

  loadCitizens()

})

</script>


<template>

  <!-- ==================================================
       PÁGINA CIUDADANOS
       ================================================== -->

  <main class="citizens-page">


    <!-- =================================================
         CABECERA
         ================================================= -->

    <div class="page-header">

      <div>

        <p class="section-label">
          GESTIÓN
        </p>

        <h1>
          Ciudadanos
        </h1>

        <p class="description">
          Gestiona los ciudadanos registrados en MiniTaxOffice.
        </p>

      </div>


      <!-- ===============================================
           NUEVO CIUDADANO
           =============================================== -->

      <button
        v-if="!showForm"
        class="btn-primary"
        @click="openForm"
      >

        + Nuevo ciudadano

      </button>


      <!-- ===============================================
           CANCELAR
           =============================================== -->

      <button
        v-else
        class="btn-primary"
        @click="closeForm"
      >

        ✕ Cancelar

      </button>

    </div>



    <!-- =================================================
         FORMULARIO
         ================================================= -->

    <section
      v-if="showForm"
      class="form-card"
    >


      <!-- ===============================================
           TÍTULO DINÁMICO
           =============================================== -->

      <!--
        Si editingId === null:

        Nuevo ciudadano

        Si editingId tiene un ID:

        Editar ciudadano
      -->

      <h2>

        {{
          editingId === null
            ? 'Nuevo ciudadano'
            : 'Editar ciudadano'
        }}

      </h2>


      <!-- DESCRIPCIÓN DINÁMICA -->

      <p class="form-description">

        {{
          editingId === null
            ? 'Introduce los datos del nuevo ciudadano.'
            : 'Modifica los datos del ciudadano seleccionado.'
        }}

      </p>



      <!-- =================================================
           FORMULARIO
           ================================================= -->

      <!--
        IMPORTANTE:

        ANTES teníamos:

        @submit.prevent="createCitizen"

        Eso provocaba que al editar
        se hiciera POST.

        AHORA utilizamos:

        saveCitizen()

        que decide POST o PUT.
      -->

      <form @submit.prevent="saveCitizen">


        <!-- =============================================
             NOMBRE
             ============================================= -->

        <div class="form-group">

          <label>
            Nombre
          </label>

          <input
            v-model="newCitizen.name"
            type="text"
            placeholder="Ej. Samira"
            required
          >

        </div>


        <!-- =============================================
             DNI
             ============================================= -->

        <div class="form-group">

          <label>
            DNI
          </label>

          <input
            v-model="newCitizen.dni"
            type="text"
            placeholder="Ej. Y7654321H"
            required
          >

        </div>


        <!-- =============================================
             EMAIL
             ============================================= -->

        <div class="form-group">

          <label>
            Email
          </label>

          <input
            v-model="newCitizen.email"
            type="email"
            placeholder="Ej. samira@email.com"
            required
          >

        </div>


        <!-- =============================================
             CIUDAD
             ============================================= -->

        <div class="form-group">

          <label>
            Ciudad
          </label>

          <input
            v-model="newCitizen.city"
            type="text"
            placeholder="Ej. Alicante"
            required
          >

        </div>



        <!-- =============================================
             BOTONES
             ============================================= -->

        <div class="form-actions">


          <!-- CANCELAR -->

          <button
            type="button"
            class="btn-secondary"
            @click="closeForm"
          >

            Cancelar

          </button>



          <!-- ===========================================
               GUARDAR
               =========================================== -->

          <!--
            El texto también cambia.

            CREAR:
            Guardar ciudadano

            EDITAR:
            Guardar cambios
          -->

          <button
            type="submit"
            class="btn-primary"
          >

            {{
              editingId === null
                ? 'Guardar ciudadano'
                : 'Guardar cambios'
            }}

          </button>

        </div>

      </form>

    </section>



    <!-- =================================================
         TABLA
         ================================================= -->

    <section class="table-card">


      <!-- CABECERA -->

      <div class="table-title">

        <h2>
          Lista de ciudadanos
        </h2>

        <p>
          {{ citizens.length }} ciudadanos registrados
        </p>

      </div>



      <!-- =================================================
           TABLA
           ================================================= -->

      <div class="table-container">

        <table>


          <!-- ===========================================
               CABECERA
               =========================================== -->

          <thead>

            <tr>

              <th>
                NOMBRE
              </th>

              <th>
                DNI
              </th>

              <th>
                EMAIL
              </th>

              <th>
                CIUDAD
              </th>

              <th>
                ACCIONES
              </th>

            </tr>

          </thead>



          <!-- ===========================================
               CUERPO
               =========================================== -->

          <tbody>


            <!--
              v-for:

              repite esta fila por cada ciudadano.

              :key:

              identifica cada fila mediante su ID.
            -->

            <tr
              v-for="citizen in citizens"
              :key="citizen.id"
            >


              <!-- NOMBRE -->

              <td>

                <div class="citizen-name">

                  <!-- Primera letra -->

                  <div class="avatar">

                    {{
                      citizen.name
                        .charAt(0)
                        .toUpperCase()
                    }}

                  </div>


                  <!-- Nombre completo -->

                  <span>
                    {{ citizen.name }}
                  </span>

                </div>

              </td>



              <!-- DNI -->

              <td>
                {{ citizen.dni }}
              </td>



              <!-- EMAIL -->

              <td>
                {{ citizen.email }}
              </td>



              <!-- CIUDAD -->

              <td>
                {{ citizen.city }}
              </td>



              <!-- =======================================
                   ACCIONES
                   ======================================= -->

              <td>

                <div class="actions">


                  <!-- EDITAR -->

                  <!--
                    Enviamos TODO el ciudadano.

                    Ejemplo:

                    editCitizen({
                      id: 2,
                      name: "Rama",
                      ...
                    })
                  -->

                  <button
                    class="btn-edit"
                    @click="editCitizen(citizen)"
                  >

                    ✏️ Editar

                  </button>



                  <!-- ELIMINAR -->

                  <!--
                    Solamente necesitamos enviar el ID.
                  -->

                  <button
                    class="btn-delete"
                    @click="deleteCitizen(citizen.id)"
                  >

                    🗑️ Eliminar

                  </button>

                </div>

              </td>

            </tr>



            <!-- =========================================
                 SIN CIUDADANOS
                 ========================================= -->

            <tr v-if="citizens.length === 0">

              <td
                colspan="5"
                class="empty-message"
              >

                No hay ciudadanos registrados.

              </td>

            </tr>

          </tbody>

        </table>

      </div>

    </section>

  </main>

</template>


<style scoped>

/* =====================================================
   PÁGINA
   ===================================================== */

.citizens-page {

  width: 100%;

  padding: 70px 75px;

  background-color: #f4f7fc;

  box-sizing: border-box;

  min-height: 100vh;

  color: #14213d;

}


/* =====================================================
   CABECERA
   ===================================================== */

.page-header {

  display: flex;

  justify-content: space-between;

  align-items: center;

  margin-bottom: 45px;

}


.section-label {

  color: #2563eb;

  font-weight: bold;

  letter-spacing: 3px;

  margin: 0 0 20px 0;

}


.page-header h1 {

  font-size: 52px;

  margin: 0 0 20px 0;

  font-weight: 600;

}


.description {

  font-size: 20px;

  color: #64748b;

  margin: 0;

}


/* =====================================================
   BOTÓN PRINCIPAL
   ===================================================== */

.btn-primary {

  background-color: #2563eb;

  color: white;

  border: none;

  border-radius: 10px;

  padding: 16px 25px;

  font-size: 16px;

  font-weight: bold;

  cursor: pointer;

}


.btn-primary:hover {

  background-color: #1d4ed8;

}


/* =====================================================
   FORMULARIO
   ===================================================== */

.form-card {

  background-color: white;

  border: 1px solid #dbe3ef;

  border-radius: 16px;

  padding: 35px;

  margin-bottom: 35px;

}


.form-card h2 {

  margin-top: 0;

  margin-bottom: 8px;

  font-size: 27px;

}


.form-description {

  color: #64748b;

  margin-bottom: 30px;

}


.form-group {

  display: flex;

  flex-direction: column;

  margin-bottom: 20px;

}


.form-group label {

  font-weight: 600;

  margin-bottom: 8px;

}


.form-group input {

  padding: 14px;

  border: 1px solid #cbd5e1;

  border-radius: 8px;

  font-size: 16px;

}


.form-group input:focus {

  outline: none;

  border-color: #2563eb;

}


.form-actions {

  display: flex;

  justify-content: flex-end;

  gap: 15px;

  margin-top: 25px;

}


.btn-secondary {

  background-color: white;

  border: 1px solid #cbd5e1;

  border-radius: 8px;

  padding: 14px 20px;

  cursor: pointer;

}


/* =====================================================
   TARJETA TABLA
   ===================================================== */

.table-card {

  background-color: white;

  border: 1px solid #dbe3ef;

  border-radius: 16px;

  overflow: hidden;

}


.table-title {

  padding: 30px;

}


.table-title h2 {

  margin: 0 0 12px 0;

  font-size: 26px;

  font-weight: 500;

}


.table-title p {

  color: #94a3b8;

  margin: 0;

}


/* =====================================================
   TABLA
   ===================================================== */

.table-container {

  overflow-x: auto;

}


table {

  width: 100%;

  border-collapse: collapse;

}


thead {

  background-color: #f8fafc;

}


th {

  text-align: left;

  padding: 20px 30px;

  color: #64748b;

  font-size: 14px;

  font-weight: 500;

  border-top: 1px solid #e2e8f0;

  border-bottom: 1px solid #e2e8f0;

}


td {

  padding: 25px 30px;

  border-bottom: 1px solid #e2e8f0;

  font-size: 16px;

}


tbody tr:last-child td {

  border-bottom: none;

}


/* =====================================================
   NOMBRE + AVATAR
   ===================================================== */

.citizen-name {

  display: flex;

  align-items: center;

  gap: 15px;

}


.avatar {

  width: 44px;

  height: 44px;

  border-radius: 50%;

  background-color: #e8efff;

  color: #2563eb;

  display: flex;

  align-items: center;

  justify-content: center;

  font-weight: bold;

}


/* =====================================================
   ACCIONES
   ===================================================== */

.actions {

  display: flex;

  gap: 10px;

}


.btn-edit,
.btn-delete {

  background-color: white;

  padding: 10px 14px;

  border-radius: 8px;

  cursor: pointer;

  font-size: 14px;

}


/* EDITAR */

.btn-edit {

  color: #2563eb;

  border: 1px solid #bfdbfe;

}


.btn-edit:hover {

  background-color: #eff6ff;

}


/* ELIMINAR */

.btn-delete {

  color: #dc2626;

  border: 1px solid #fecaca;

}


.btn-delete:hover {

  background-color: #fff1f2;

}


/* =====================================================
   TABLA VACÍA
   ===================================================== */

.empty-message {

  text-align: center;

  color: #94a3b8;

  padding: 40px;

}

</style>