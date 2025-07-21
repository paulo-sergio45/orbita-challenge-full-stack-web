import { createRouter, createMemoryHistory } from "vue-router";
import StudentList from "../components/StudentList.vue";
import StudentForm from "../components/StudentForm.vue";

const routes = [
  { path: "/", redirect: "/students" },
  { path: "/students", component: StudentList },
  { path: "/students/new", component: StudentForm },
  { path: "/students/:id/edit", component: StudentForm },
];

const router = createRouter({
  history: createMemoryHistory(),
  routes,
});

export default router;
