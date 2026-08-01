console.log(process.env.VUE_APP_API_URL);

import axios from "axios";

export default axios.create({
    baseURL: process.env.VUE_APP_API_URL
});