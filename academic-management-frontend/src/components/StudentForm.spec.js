import { mount, flushPromises } from "@vue/test-utils";
import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import StudentFormPage from "../components/StudentFormPage.vue";
import { createRouter, createMemoryHistory } from "vue-router";
import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import * as studentService from "../services/studentService";

vi.mock("../services/studentService", () => ({
  fetchStudentById: vi.fn(),
  createStudent: vi.fn(),
  updateStudent: vi.fn(),
}));

const vuetify = createVuetify({
  components,
  directives,
});

const mockStudent = {
  id: 1,
  name: "João Silva",
  email: "joao@email.com",
  ra: "123",
  cpf: "11111111111",
};

const mockNewStudent = {
  name: "Maria",
  email: "maria@email.com",
  ra: "456",
  cpf: "222.222.222-22",
};

const createTestRouter = () => {
  return createRouter({
    history: createMemoryHistory(),
    routes: [
      { path: "/students/new", component: StudentFormPage },
      { path: "/students/:id/edit", component: StudentFormPage },
      { path: "/students", component: {} },
    ],
  });
};

const mountComponent = async (route = "/students/new", options = {}) => {
  const router = createTestRouter();
  await router.push(route);
  await router.isReady();

  const wrapper = mount(StudentFormPage, {
    global: { plugins: [router, vuetify] },
    ...options,
  });

  await flushPromises();
  await wrapper.vm.$nextTick();

  return wrapper;
};

const fillForm = async (wrapper, studentData) => {
  await wrapper.find("#input-name").setValue(studentData.name);
  await wrapper.find("#input-email").setValue(studentData.email);
  await wrapper.find("#input-ra").setValue(studentData.ra);
  await wrapper.find("#input-cpf").setValue(studentData.cpf);
  await flushPromises();
};

describe("StudentFormPage.vue", () => {
  let router;

  beforeEach(async () => {
    router = createTestRouter();
    await router.push("/students/new");
    await router.isReady();

    vi.mocked(studentService.fetchStudentById).mockResolvedValue(mockStudent);
    vi.mocked(studentService.createStudent).mockResolvedValue({});
    vi.mocked(studentService.updateStudent).mockResolvedValue({});
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  describe("Renderização", () => {
    it("deve renderizar formulário com campos vazios para novo aluno", async () => {
      const wrapper = await mountComponent();

      expect(wrapper.find("#input-name").element.value).toBe("");
      expect(wrapper.find("#input-email").element.value).toBe("");
      expect(wrapper.find("#input-ra").element.value).toBe("");
      expect(wrapper.find("#input-cpf").element.value).toBe("");
    });

    it("deve carregar dados existentes para edição", async () => {
      const wrapper = await mountComponent("/students/1/edit");

      // Verificar se fetchStudentById foi chamado
      expect(studentService.fetchStudentById).toHaveBeenCalledWith("1");

      expect(wrapper.find("#input-name").element.value).toBe(mockStudent.name);
      expect(wrapper.find("#input-email").element.value).toBe(
        mockStudent.email
      );
      expect(wrapper.find("#input-ra").element.value).toBe(mockStudent.ra);
      expect(wrapper.find("#input-cpf").element.value).toBe(mockStudent.cpf);
    });
  });

  describe("Criação de aluno", () => {
    it("deve chamar createStudent ao salvar novo aluno", async () => {
      const wrapper = await mountComponent();

      await fillForm(wrapper, mockNewStudent);
      expect(wrapper.vm.valid).toBe(true);

      await wrapper.find("#student-form").trigger("submit.prevent");
      await flushPromises();

      expect(studentService.createStudent).toHaveBeenCalledWith(mockNewStudent);
    });

    it("deve navegar para /students após criar aluno", async () => {
      const wrapper = await mountComponent();
      const routerPushSpy = vi.spyOn(wrapper.vm.$router, "push");

      await fillForm(wrapper, mockNewStudent);
      await wrapper.find("#student-form").trigger("submit.prevent");
      await flushPromises();

      expect(routerPushSpy).toHaveBeenCalledWith("/students");
    });
  });

  describe("Edição de aluno", () => {
    it("deve chamar updateStudent ao salvar edição", async () => {
      const wrapper = await mountComponent("/students/1/edit");
      const updatedName = "João Atualizado";

      await wrapper.find("#input-name").setValue(updatedName);
      await wrapper.find("#student-form").trigger("submit.prevent");
      await flushPromises();

      expect(studentService.updateStudent).toHaveBeenCalledWith(
        "1",
        expect.objectContaining({
          name: updatedName,
          email: mockStudent.email,
          ra: mockStudent.ra,
          cpf: mockStudent.cpf,
        })
      );
    });

    it("deve navegar para /students após editar aluno", async () => {
      const wrapper = await mountComponent("/students/1/edit");
      const routerPushSpy = vi.spyOn(wrapper.vm.$router, "push");

      await wrapper.find("#student-form").trigger("submit.prevent");
      await flushPromises();

      expect(routerPushSpy).toHaveBeenCalledWith("/students");
    });
  });

  describe("Navegação", () => {
    it("deve navegar para /students ao cancelar", async () => {
      const wrapper = await mountComponent();
      const routerPushSpy = vi.spyOn(wrapper.vm.$router, "push");

      await wrapper.find("#btn-cancel").trigger("click");

      expect(routerPushSpy).toHaveBeenCalledWith("/students");
    });
  });

  describe("Validação", () => {
    it("deve validar campos obrigatórios", async () => {
      const wrapper = await mountComponent();

      await wrapper.find("#student-form").trigger("submit.prevent");
      await flushPromises();

      // Verifica se a validação foi acionada
      expect(wrapper.vm.valid).toBe(false);
    });

    it("deve permitir envio quando formulário é válido", async () => {
      const wrapper = await mountComponent();

      await fillForm(wrapper, mockNewStudent);
      expect(wrapper.vm.valid).toBe(true);
    });
  });

  describe("Comportamento dos campos", () => {
    it("deve desabilitar RA e CPF em modo de edição", async () => {
      const wrapper = await mountComponent("/students/1/edit");

      const raField = wrapper.find("#input-ra");
      const cpfField = wrapper.find("#input-cpf");

      expect(raField.attributes("disabled")).toBeDefined();
      expect(cpfField.attributes("disabled")).toBeDefined();
    });

    it("deve habilitar RA e CPF em modo de criação", async () => {
      const wrapper = await mountComponent();

      const raField = wrapper.find("#input-ra");
      const cpfField = wrapper.find("#input-cpf");

      expect(raField.attributes("disabled")).toBeUndefined();
      expect(cpfField.attributes("disabled")).toBeUndefined();
    });
  });
});
