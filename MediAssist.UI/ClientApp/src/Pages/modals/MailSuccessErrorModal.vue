<template>
<div v-if="isVisible" class="modal-overlay">
  <div class="mail-box">
    <div class="modal-header">
        <button class="close-button" @click="cancel">
        <span>&times;</span>
        </button>
    </div>
    <div class="mail-success">
      <div v-if="isError" class="error-icon">  
        <img class="mail-sent" src="@/assets/error-image.gif" :alt="errorAlt" />
      </div>
      <div v-else class="success-icon">
        <img class="mail-sent" src="@/assets/mail-sent.gif" :alt="successAlt" />
      </div>
      <p class="message">
        {{ message }}
      </p>
    </div>
  </div>
</div>
</template>

<script>
export default {
  props: {
    isVisible: {
      type: Boolean,
      required: true,
    },
    isError: {
      type: Boolean,
      required: true,
    },
    successAlt: {
      type: String,
      default: "Mail Sent Successfully",
    },
    errorAlt: {
      type: String,
      default: "An Error Occurred",
    },
    message: {
      type: String,
      required: true,
    },
  },
  methods: {
    cancel() {
      this.$emit("cancel");
    },
  },
};
</script>

<style scoped>
.modal-overlay {
   position: fixed;
   top: 0;
   left: 0;
   width: 100%;
   height: 100%;
   background-color: rgba(0, 0, 0, 0.5);
   display: flex;
   justify-content: center;
   align-items: flex-start;
   z-index: 1000;
 }
.mail-success {
  display: flex;
  flex-direction: column;
  align-items: center;
  height: 100%;
  width: 100%;
  padding-top: 20px;
}
.mail-sent {
  height: 179px;
  width: auto;
  margin-bottom: 20px; 
}
.success-message {
   width: 440px;
  text-align: center;
}
.mail-box{
  width: 450px;
  height: auto;
  top: 175px;
  left: 35%;
  padding: 40px 40px 40px 40px;
  gap: 30px;
  border-radius: 17px;
  background: #ffffff;
  flex-direction: column;
  position: absolute;  
}
.modal-header{
   width: 100%;
 }
 .close-button {
   width: 32px;
   height: 32px;
   position: relative;
   font-size: 35px;
   cursor: pointer;
   color: #aaa;
   border: none;
   background: none;
   float: right;
 }
 .close-button:hover {
   color: black;
 }
</style>