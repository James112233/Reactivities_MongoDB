import { rejects } from "assert";
import axios, { AxiosResponse } from "axios";
import { Activity } from "../models/activity";

const sleep = (delay: number) => {
    return new Promise((resole) => {
        setTimeout(resole, delay)
    })
}

axios.defaults.baseURL = "http://localhost:5000/api";

const responseBody = (response: AxiosResponse) => response.data;

axios.interceptors.response.use(async response => {
    try {
        await sleep(500);
        return response;
    } catch (error) {
        console.log(error);
        return await Promise.reject(error);
    }
})


const request = {
    get: (url: string) => axios.get(url).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    del: (url: string) => axios.delete(url).then(responseBody),
}

const Activities = {
    list: () => request.get('/activity'),
    details: (id: string) => request.get(`/activity/${id}`),
    create: (activity: Activity) => axios.post('/activity', activity),
    update: (activity: Activity) => axios.put("/activity", activity),
    delete: (id: string) => axios.delete(`/activity/${id}`)
}

const agent = {
    Activities
}

export default agent;