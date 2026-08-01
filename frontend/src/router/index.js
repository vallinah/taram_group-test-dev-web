import Vue from 'vue'
import VueRouter from 'vue-router'
import ContactsView from '../views/ContactsView.vue'


Vue.use(VueRouter)


const routes = [
{
    path:'/',
    name:'contacts',
    component:ContactsView
}
]


const router = new VueRouter({
    mode:'history',
    routes
})


export default router