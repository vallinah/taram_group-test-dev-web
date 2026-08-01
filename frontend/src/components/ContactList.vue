<template>
<div class="container">

    <div class="card">

    <h1>Contacts</h1>

    <ImportButton @imported="loadContacts"/>

    </div>

    <input
        v-model="search"
        placeholder="Rechercher..."
    />

    <button @click="loadContacts">
        Rechercher
    </button>


    <div class="card">
        <table border="1">

            <thead>
                <tr>
                    <th>Société</th>
                    <th>Contact</th>
                    <th>Email</th>
                    <th>Action</th>
                </tr>
            </thead>


            <tbody>

                <tr
                    v-for="contact in contacts"
                    :key="contact.id"
                >

                    <td>{{ contact.societe }}</td>

                    <td>{{ contact.nomContact }}</td>

                    <td>{{ contact.email }}</td>

                    <td>
                        <button
                            @click="selectContact(contact)"
                        >
                            Modifier
                        </button>
                    </td>

                </tr>

            </tbody>

        </table>
    </div>

    <ContactForm
        v-if="selectedContact"
        :contact="selectedContact"
        @updated="loadContacts"
    />


    <div class="pagination">

        <button
            @click="previousPage"
            :disabled="page === 1"
        >
            Précédent
        </button>


        <span>
            Page {{ page }} / {{ totalPages }}
        </span>


        <button
            @click="nextPage"
            :disabled="page === totalPages"
        >
            Suivant
        </button>

    </div>


</div>
</template>


<script>

import api from "@/api/axios";
import ContactForm from "@/components/ContactForm.vue";
import ImportButton from "@/components/ImportButton.vue";


export default {

components:{
    ContactForm,
    ImportButton
},


data(){

    return {
        contacts: [],
        search: "",
        page: 1,
        pageSize: 5,
        totalPages: 0,
        selectedContact:null
    }

},

methods:{

    async loadContacts(){

        const response = await api.get(
            "/contacts",
            {
                params:{
                    search:this.search,
                    page:this.page,
                    pageSize:this.pageSize
                }
            }
        );

        this.contacts = response.data.items;
        this.totalPages = response.data.totalPages;

    },


    selectContact(contact){

        this.selectedContact = contact;

    },


    nextPage(){

        if(this.page < this.totalPages)
        {
            this.page++;
            this.loadContacts();
        }

    },


    previousPage(){

        if(this.page > 1)
        {
            this.page--;
            this.loadContacts();
        }

    }

},


mounted(){

    this.loadContacts();

}


}

</script>