<template>
    <div class="feature-card">
        <img :src="icon" alt="Feature Icon" class="feature-icon" />
        <div class="feature-details">
            <h3 class="feature-title">{{ title }}</h3>
            <p class="feature-description">{{ description }}</p>
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            icon: String,
            title: String,
            description: String,
        },
    };
</script>

<style scoped>
    .feature-card {
        text-align: center;
        background-color: #fff;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        height: auto;
        min-height: 290px;
        margin-bottom: 93px;
        flex: 1 1 20%;
        margin-right: 3%;
    }

        .feature-card:hover {
            transform: translateY(-5px);
        }

    .feature-icon {
        width: 100%;
        height: 170.446px;
        margin-bottom: 10px;
        object-fit: cover;
        border-radius: 14.54px;
        margin-top: -19px;
    }

    .feature-details {
        text-align: left;
        padding: 4px 11px 11px 20px;
    }

    .feature-title {
        margin-bottom: 10px;
        align-self: stretch;
        color: #1C1C1C;
        font-size: 14.54px;
        font-style: normal;
        font-weight: 600;
        line-height: 150%;
        font-family: Inter;
    }


.feature-description {
    width: 275.46px;
    margin-bottom: 15px;
    color: #353535;
    font-family: Inter;
    font-style: normal;
    font-weight: 400;
    line-height: 150%;
    font-size: 12px;
}

    .consult-now {
        color: #13bef0;
        text-decoration: none;
        font-weight: bold;
    }

    @media screen and (max-width: 420px) {

        .feature-card {
            margin-bottom: 18%;
            flex: 1 1 100%;
        }
    }

    @media screen and (min-width: 724px) and (max-width: 1026px) {

        .feature-card {
            margin-bottom: 8%;
            flex: 1 1 25%;
        }
    }

    @media screen and (min-width: 1026px) and (max-width: 1417px) {
        .feature-card {
            margin-bottom: 8%;
            flex: 1 1 33.33%
        }
    }
</style>
