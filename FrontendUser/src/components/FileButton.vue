<template>
    <!-- <input type="file"> -->
    <input 
    style="display: none;" 
    type="file" 
    @change="onFileSelected"
    ref="fileInput">
    <button class="file-button" @click="$refs.fileInput.click()">Pick file</button>
    <button class="file-button" @click="onUpload">Upload</button>
</template>

<script>
import axios from 'axios';

export default{
name: 'FileButton',
data () {
    return{
        selectedFile : null
    }
},
methods: {
    onFileSelected(event) {
        this.selectedFile = event.target.files[0]
    },
    onUpload () {
        const fd = new FormData();
        fd.append('image', this.selectedFile, this.selectedFile.name)
     axios.post('api', fd, {
        onUploadProgress: uploadEvent => {
            console.log('Upload progress: ' + Math.round(uploadEvent.loaded / uploadEvent.total * 100) + '%')
        }
     }).then(res => {
        console.log(res)
     })   // api link npm
    }
}
}

</script>

<style scoped>
.file-button {
width: 160px;
height: 40px;
border: 1px solid orange;
background-color: orange;
color: #fff;
cursor: pointer;
border-radius: 15px;
}
</style>