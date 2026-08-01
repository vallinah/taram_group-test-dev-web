<template>
<div>

    <button @click="importContacts">
        Lancer l'import
    </button>


    <div v-if="report">

        <h3>Rapport d'import</h3>

        <p>
            Créés : {{ report.created }}
        </p>

        <p>
            Mis à jour : {{ report.updated }}
        </p>

        <p>
            Rejetés : {{ report.rejected }}
        </p>


        <div v-if="report.errors.length">

            <h4>Erreurs</h4>

            <ul>

                <li
                    v-for="error in report.errors"
                    :key="error.line"
                >

                    Ligne {{ error.line }} -
                    {{ error.reason }}

                </li>

            </ul>

        </div>


    </div>


    <p
        v-if="error"
        style="color:red"
    >
        {{ error }}
    </p>


</div>
</template>


<script>

import api from "@/api/axios";


export default {


data(){

    return {

        report:null,

        error:null

    }

},


methods:{


async importContacts(){

    this.error=null;


    try{

        const response = await api.post(
            "/import"
        );


        this.report = response.data;


        this.$emit(
            "imported"
        );


    }
    catch(error){

        this.error =
            "Erreur pendant l'import";

    }

}


}


}

</script>