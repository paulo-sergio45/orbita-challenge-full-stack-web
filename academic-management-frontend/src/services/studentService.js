import axios from "axios";

const API_URL = "https://localhost:7246/v1/students";

export async function fetchStudentById(id) {
  const response = await axios.get(`${API_URL}/${id}`);
  debugger;
  return response.data;
}

export async function createStudent(student) {
  debugger;
  const response = await axios.post(API_URL, student);
  return response.data;
}

export async function updateStudent(id, student) {
  const response = await axios.put(`${API_URL}/${id}`, student);
  return response.data;
}

export async function deleteStudent(id) {
  await axios.delete(`${API_URL}/${id}`);
}

export async function fetchStudentsPaged(
  pageNumber = 1,
  pageSize = 10,
  search = "",
  sortBy = null,
  sortDesc = false
) {
  const params = { pageNumber, pageSize, search };
  if (sortBy) params.sortBy = sortBy;
  if (sortDesc !== undefined) params.sortDesc = sortDesc;
  const response = await axios.get(`${API_URL}/paged`, { params });
  return response.data;
}
