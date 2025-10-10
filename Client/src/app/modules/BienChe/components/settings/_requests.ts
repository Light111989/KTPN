import axios from 'axios'
import { DonVi } from '../settings/_models'
import { BienChePaging } from './BienChePaging'

const API_URL = process.env.REACT_APP_API_URL
// get data
export function fetchDonVis(page: number, pageSize: number) {
  return axios.post(`${API_URL}/BienChes/listing?page=${page}&pageSize=${pageSize}`)

}

export const deleteDonVi = async (id: string | number) => {
  return axios.delete(`${API_URL}/BienChes/${id}`)
}

export async function updateDonVi(id: string, data: any) {
  const res = await fetch(`http://localhost:5000/api/BienChes/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!res.ok) {
    const err = await res.json();
    throw new Error(err.message || 'Update thất bại');
  }
  return res;
}

export const createDonVi = async (data: any) => {
  const res = await axios.post(`${API_URL}/BienChes`, data);
  return res.data;
};

export function fetchLinhVucs() {
  return axios.post(`${API_URL}/LinhVucs/listing?`)
}

export function fetchKhois() {
  return axios.post(`${API_URL}/Khois/listing?`)
}

export const searchBienChes = async (filters: { tenDonVi?: string; khoiId?: string; linhVucId?: string }) => {
  const res = await axios.get(`${API_URL}/BienChes/search`, { params: filters });
  return res.data;
};

export async function exportExcel() {
  const response = await axios.get(`${API_URL}/BienChes/export-excel`, {
    responseType: 'blob', // BẮT BUỘC để nhận file
  })

  // tạo link download
  const url = window.URL.createObjectURL(new Blob([response.data]))
  const link = document.createElement('a')
  link.href = url
  link.setAttribute('download', `BienChe_${new Date().getTime()}.xlsx`)
  document.body.appendChild(link)
  link.click()
  link.remove()
}