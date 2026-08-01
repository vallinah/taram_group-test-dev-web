<template>
<div v-if="contact" class="card">

    <h2>Modifier le contact</h2>


    <div>
        <label>Société</label>
        <input v-model="form.societe">
    </div>


    <div>
        <label>Nom contact</label>
        <input v-model="form.nomContact">
    </div>


    <div>
        <label>Email</label>
        <input v-model="form.email">
    </div>


    <div>
        <label>Téléphone</label>
        <input v-model="form.telephone">
    </div>


    <div>
        <label>CA annuel</label>
        <input
            type="number"
            v-model="form.caAnnuel"
        >
    </div>


    <button @click="updateContact">
        Enregistrer
    </button>


    <p
      v-if="error"
      style="color:red"
    >
        {{ error }}
    </p>


    <p
      v-if="success"
      style="color:green"
    >
        Modification réussie
    </p>


</div>
</template>


<script>

import api from "@/api/axios";


export default {

props:{
    contact:{
        type:Object,
        required:true
    }
},


data(){

    return {

        form:{
            societe:"",
            nomContact:"",
            email:"",
            telephone:"",
            caAnnuel:null
        },

        error:null,

        success:false

    }

},


watch:{

    contact:{
        immediate:true,

        handler(value){

            if(value)
            {
                this.form={
                    societe:value.societe,
                    nomContact:value.nomContact,
                    email:value.email,
                    telephone:value.telephone,
                    caAnnuel:value.caAnnuel
                }
            }

        }
    }

},


methods:{


async updateContact(){

    this.error=null;
    this.success=false;


    try{

        await api.put(
            `/contacts/${this.contact.id}`,
            this.form
        );


        this.success=true;


        this.$emit("updated");


    }
    catch(error){

        if(error.response)
        {
            this.error =
                error.response.data.message;
        }
        else
        {
            this.error="Erreur serveur";
        }

    }

}


}

}

</script>