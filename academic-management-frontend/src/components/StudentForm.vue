<script setup>
import { ref, onMounted, computed } from "vue";
import { useRoute, useRouter } from "vue-router";
import {
  fetchStudentById,
  createStudent,
  updateStudent,
} from "../services/studentService";

const route = useRoute();
const router = useRouter();

const form = ref({
  name: "",
  email: "",
  ra: "",
  cpf: "",
});

const valid = ref(false);
const formRef = ref(null);

const isEdit = computed(() => !!route.params.id);

const validationRules = {
  required: (value) => !!value || "Campo obrigatório",
  email: (value) => /.+@.+\..+/.test(value) || "E-mail inválido",
};

const formFields = computed(() => [
  {
    key: "name",
    label: "Nome",
    placeholder: "Informe o nome completo",
    rules: [validationRules.required],
    disabled: false,
  },
  {
    key: "email",
    label: "Email",
    placeholder: "Informe apenas um e-mail",
    rules: [validationRules.required, validationRules.email],
    type: "email",
    disabled: false,
  },
  {
    key: "ra",
    label: "RA",
    placeholder: "Informe o registro acadêmico",
    rules: [validationRules.required],
    disabled: isEdit.value,
  },
  {
    key: "cpf",
    label: "CPF",
    placeholder: "Informe o número do documento acadêmico",
    rules: [validationRules.required],
    disabled: isEdit.value,
  },
]);

async function loadStudentData(studentId) {
  try {
    const data = await fetchStudentById(studentId);
    Object.keys(form.value).forEach((key) => {
      if (data[key]) {
        form.value[key] = data[key];
      }
    });
  } catch (error) {
    console.error("Erro ao carregar dados do aluno:", error);
  }
}

async function saveFormData() {
  try {
    if (isEdit.value) {
      await updateStudent(route.params.id, form.value);
    } else {
      await createStudent(form.value);
    }
    return true;
  } catch (error) {
    console.error("Erro ao salvar dados:", error);
    return false;
  }
}

async function handleSave() {
  const isValid = await formRef.value?.validate();

  if (formRef.value && isValid) {
    const success = await saveFormData();
    if (success) {
      router.push("/students");
    }
  }
}

function handleCancel() {
  router.push("/students");
}

onMounted(async () => {
  if (isEdit.value) {
    await loadStudentData(route.params.id);
  }
});
</script>

<template>
  <v-container>
    <v-form @submit.prevent="handleSave" ref="formRef" v-model="valid" id="student-form">
      <v-text-field v-for="field in formFields" :key="field.key" v-model="form[field.key]" :label="field.label"
        :rules="field.rules" :disabled="field.disabled" :type="field.type" :placeholder="field.placeholder"
        :required="field.rules.includes(validationRules.required)" :id="`input-${field.key}`" />

      <v-row class="mt-4">
        <v-spacer />
        <v-btn color="primary" type="submit" id="btn-save" :loading="false">
          {{ isEdit ? "Atualizar" : "Salvar" }}
        </v-btn>
        <v-spacer />
        <v-btn color="secondary" @click="handleCancel" id="btn-cancel">
          Cancelar
        </v-btn>
        <v-spacer />
      </v-row>
    </v-form>
  </v-container>
</template>

<style scoped>
.v-container {
  max-width: 600px;
  margin: 0 auto;
}

.v-form {
  padding: 20px;
}
</style>
