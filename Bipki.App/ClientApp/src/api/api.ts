import axios from 'axios';
export const BACKEND_URL = '/api';
const REQUEST_TIMEOUT = 5000;


export const api = axios.create({
  baseURL: BACKEND_URL,
  timeout: REQUEST_TIMEOUT,
  headers: {
    "X-Requested-With": "XMLHttpRequest"
  }
});