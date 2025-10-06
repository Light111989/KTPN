import axios from 'axios'
import {DonVi} from '../settings/_models'
import { BienChePaging } from './BienChePaging'

const API_URL = process.env.REACT_APP_API_URL
// get data
export function fetchDonVis(page: number, pageSize: number) {
  return axios.post(`${API_URL}/BienChes/listing?page=${page}&pageSize=${pageSize}`)

}

export const deleteDonVi = async (id: string | number) => {
  return axios.delete(`${API_URL}/BienChes/${id}`)
}

export const createDonVi = async (data: any) => {
  const res = await axios.post(`${API_URL}/BienChes`, data);
  return res.data;
};

export function fetchLinhVucs(page: number, pageSize: number) {
  return axios.post(`${API_URL}/LinhVucs/listing?page=${page}&pageSize=${pageSize}`)

}