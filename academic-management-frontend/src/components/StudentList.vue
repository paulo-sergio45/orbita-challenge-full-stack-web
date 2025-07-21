<script setup>
import { ref, watch } from "vue";
import { useRouter } from "vue-router";
import { fetchStudentsPaged, deleteStudent } from "../services/studentService";

const router = useRouter();

const students = ref([]);
const totalItems = ref(0);
const pageSize = ref(10);
const loading = ref(false);
const search = ref("");
const showDeleteDialog = ref(false);
const selectedStudentId = ref(null);

const tableHeaders = ref([
  { title: "Nome", key: "name", align: "start" },
  { title: "Email", key: "email", align: "start" },
  { title: "RA", key: "ra", align: "start" },
  { title: "CPF", key: "cpf", align: "start" },
  { title: "Ações", key: "actions", align: "center", sortable: false },
]);

function navigateToEdit(studentId) {
  router.push(`/students/${studentId}/edit`);
}

function openDeleteConfirmation(studentId) {
  selectedStudentId.value = studentId;
  showDeleteDialog.value = true;
}

function closeDeleteConfirmation() {
  showDeleteDialog.value = false;
  selectedStudentId.value = null;
}

async function confirmDelete() {
  await deleteStudent(selectedStudentId.value);
  loadItems({ page: 1, itemsPerPage: pageSize.value });
  closeDeleteConfirmation();
}

function loadItems({ page, itemsPerPage, sortBy }) {
  loading.value = true;
  let sortKey = sortBy?.[0]?.key || null;
  let sortDesc = sortBy?.[0]?.order === "desc" || false;
  fetchStudentsPaged(page, itemsPerPage, search.value, sortKey, sortDesc).then(
    (result) => {
      students.value = result.items;
      totalItems.value = result.totalItems;
      loading.value = false;
    }
  );
}

watch(search, () => {
  loadItems({ page: 1, itemsPerPage: pageSize.value });
});
</script>

<template>
  <v-container>
    <v-row class="mb-4">
      <v-col cols="12" md="8">
        <v-text-field v-model="search" label="Pesquisar aluno" prepend-inner-icon="mdi-magnify" clearable
          placeholder="Digite para pesquisar por nome, email, RA ou CPF" id="search-input" />
      </v-col>
      <v-col cols="12" md="4" class="d-flex align-center justify-end">
        <v-btn color="primary" @click="() => router.push('/students/new')" id="create-button" prepend-icon="mdi-plus">
          Cadastrar Aluno
        </v-btn>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12">
        <v-data-table-server v-model:items-per-page="pageSize" :headers="tableHeaders" :items="students"
          :items-length="totalItems" :loading="loading" item-key="id" @update:options="loadItems">
          <template #item.actions="{ item }">
            <v-btn icon size="small" color="primary" @click="navigateToEdit(item.id)">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon size="small" color="error" @click="openDeleteConfirmation(item.id)">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
          <template #no-data>
            <div class="text-center pa-4">
              <v-icon size="48" color="grey">mdi-account-group-outline</v-icon>
              <p class="text-grey mt-2">Nenhum aluno cadastrado.</p>
            </div>
          </template>
        </v-data-table-server>
      </v-col>
    </v-row>
    <v-dialog v-model="showDeleteDialog" max-width="400" id="delete-dialog" persistent>
      <v-card>
        <v-card-title class="text-h6">
          <v-icon color="error" class="mr-2">mdi-alert-circle</v-icon>
          Confirmar exclusão
        </v-card-title>

        <v-card-text>
          <p>Tem certeza que deseja excluir este aluno?</p>
          <p class="text-caption text-grey">Esta ação não pode ser desfeita.</p>
        </v-card-text>

        <v-card-actions>
          <v-spacer />
          <v-btn color="grey" variant="text" @click="closeDeleteConfirmation" id="cancelar-button">
            Cancelar
          </v-btn>
          <v-btn color="error" variant="text" @click="confirmDelete" id="excluir-button">
            Excluir
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<style scoped>
.v-container {
  max-width: 1200px;
  margin: 0 auto;
}

.gap-2 {
  gap: 8px;
}

.v-data-table-server {
  border-radius: 8px;
}

.v-btn {
  transition: all 0.2s ease;
}

.v-btn:hover {
  transform: scale(1.05);
}
</style>
