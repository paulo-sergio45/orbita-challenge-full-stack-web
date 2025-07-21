import { mount, flushPromises } from "@vue/test-utils";
import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import StudentList from "../components/StudentList.vue";
import { createRouter, createMemoryHistory } from "vue-router";
import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import * as studentService from "../services/studentService";

const vuetify = createVuetify({
  components,
  directives,
});

global.ResizeObserver = require("resize-observer-polyfill");
vi.stubGlobal("visualViewport", new EventTarget());

const mockStudents = [
  {
    id: 1,
    name: "João Silva",
    email: "joao@email.com",
    ra: "123",
    cpf: "111.111.111-11",
  },
  {
    id: 2,
    name: "Maria Oliveira",
    email: "maria@email.com",
    ra: "456",
    cpf: "222.222.222-22",
  },
  {
    id: 3,
    name: "Pedro Santos",
    email: "pedro@email.com",
    ra: "789",
    cpf: "333.333.333-33",
  },
];

vi.mock("../services/studentService", () => ({
  fetchStudents: vi.fn(),
  deleteStudent: vi.fn(),
}));

const createTestRouter = () => {
  return createRouter({
    history: createMemoryHistory(),
    routes: [
      { path: "/students", component: StudentList },
      { path: "/students/new", component: {} },
      { path: "/students/:id/edit", component: {} },
    ],
  });
};

const mountComponent = async (options = {}) => {
  const router = createTestRouter();
  await router.push("/students");
  await router.isReady();

  const wrapper = mount(StudentList, {
    global: { plugins: [vuetify, router] },
    ...options,
  });

  await flushPromises();
  await wrapper.vm.$nextTick();

  return wrapper;
};

const getTableRows = (wrapper) => {
  return wrapper.findAll("tbody tr");
};

const expectTextToContain = (wrapper, text) => {
  expect(wrapper.text()).toContain(text);
};

describe("StudentList.vue", () => {
  beforeEach(async () => {
    vi.mocked(studentService.fetchStudents).mockResolvedValue(mockStudents);
    vi.mocked(studentService.deleteStudent).mockResolvedValue({});
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  describe("Renderização", () => {
    it("deve renderizar a tabela com os alunos", async () => {
      const wrapper = await mountComponent();

      expect(getTableRows(wrapper).length).toBe(3);
      expectTextToContain(wrapper, "João Silva");
      expectTextToContain(wrapper, "Maria Oliveira");
      expectTextToContain(wrapper, "Pedro Santos");
    });

    it("deve exibir todos os campos dos alunos", async () => {
      const wrapper = await mountComponent();

      const firstRow = getTableRows(wrapper)[0];
      expectTextToContain(firstRow, "João Silva");
      expectTextToContain(firstRow, "joao@email.com");
      expectTextToContain(firstRow, "123");
      expectTextToContain(firstRow, "111.111.111-11");
    });

    it("deve carregar dados do serviço", async () => {
      await mountComponent();

      expect(studentService.fetchStudents).toHaveBeenCalledTimes(1);
    });
  });

  describe("Funcionalidade de pesquisa", () => {
    it("deve filtrar alunos por nome", async () => {
      const wrapper = await mountComponent();

      const searchInput = wrapper.find("#search-input");
      await searchInput.setValue("João");
      await flushPromises();

      expect(getTableRows(wrapper).length).toBe(1);
      expectTextToContain(wrapper, "João Silva");
      expect(wrapper.text()).not.toContain("Maria Oliveira");
    });

    it("deve filtrar alunos por email", async () => {
      const wrapper = await mountComponent();

      const searchInput = wrapper.find("#search-input");
      await searchInput.setValue("maria@email.com");
      await flushPromises();

      expect(getTableRows(wrapper).length).toBe(1);
      expectTextToContain(wrapper, "Maria Oliveira");
      expect(wrapper.text()).not.toContain("João Silva");
    });

    it("deve filtrar alunos por RA", async () => {
      const wrapper = await mountComponent();

      const searchInput = wrapper.find("#search-input");
      await searchInput.setValue("456");
      await flushPromises();

      expect(getTableRows(wrapper).length).toBe(1);
      expectTextToContain(wrapper, "Maria Oliveira");
    });

    it("deve mostrar todos os alunos quando pesquisa está vazia", async () => {
      const wrapper = await mountComponent();

      const searchInput = wrapper.find("#search-input");
      await searchInput.setValue("");
      await flushPromises();

      expect(getTableRows(wrapper).length).toBe(3);
    });

    it("deve ser case-insensitive na pesquisa", async () => {
      const wrapper = await mountComponent();

      const searchInput = wrapper.find("#search-input");
      await searchInput.setValue("joão");
      await flushPromises();

      expect(getTableRows(wrapper).length).toBe(1);
      expectTextToContain(wrapper, "João Silva");
    });
  });

  describe("Navegação", () => {
    it("deve navegar para tela de cadastro ao clicar no botão", async () => {
      const wrapper = await mountComponent();
      const routerPushSpy = vi.spyOn(wrapper.vm.$router, "push");

      const createButton = wrapper.find("#create-button");
      await createButton.trigger("click");

      expect(routerPushSpy).toHaveBeenCalledWith("/students/new");
    });

    it("deve navegar para edição ao clicar no botão editar", async () => {
      const wrapper = await mountComponent();
      const routerPushSpy = vi.spyOn(wrapper.vm.$router, "push");

      const editButtons = wrapper.findAll("#edit-button");
      await editButtons[0].trigger("click");

      expect(routerPushSpy).toHaveBeenCalledWith("/students/1/edit");
    });
  });

  describe("Exclusão de alunos", () => {
    it("deve abrir diálogo de confirmação ao clicar em excluir", async () => {
      const wrapper = await mountComponent();

      const deleteButtons = wrapper.findAll("#delete-button");
      await deleteButtons[0].trigger("click");
      await flushPromises();

      expect(wrapper.vm.showDeleteDialog).toBe(true);
    });

    it("deve chamar deleteStudent e recarregar lista", async () => {
      const wrapper = await mountComponent();

      wrapper.vm.selectedStudentId = 1;
      await wrapper.vm.handleDelete();
      await flushPromises();

      expect(studentService.deleteStudent).toHaveBeenCalledWith(1);
      expect(studentService.fetchStudents).toHaveBeenCalledTimes(2);
    });

    it("deve fechar diálogo de confirmação ao cancelar exclusão", async () => {
      const wrapper = await mountComponent();

      wrapper.vm.showDeleteDialog = true;
      wrapper.vm.selectedStudentId = 1;
      await wrapper.vm.handleDelete();
      await flushPromises();

      expect(wrapper.vm.showDeleteDialog).toBe(false);
    });
  });

  describe("Comportamento da tabela", () => {
    it("deve ter botões de ação para cada linha", async () => {
      const wrapper = await mountComponent();

      const editButtons = wrapper.findAll("#edit-button");
      const deleteButtons = wrapper.findAll("#delete-button");

      expect(editButtons.length).toBe(3);
      expect(deleteButtons.length).toBe(3);
    });

    it("deve ter cabeçalhos corretos", async () => {
      const wrapper = await mountComponent();

      expectTextToContain(wrapper, "Name");
      expectTextToContain(wrapper, "Email");
      expectTextToContain(wrapper, "RA");
      expectTextToContain(wrapper, "CPF");
      expectTextToContain(wrapper, "Ações");
    });
  });
});
