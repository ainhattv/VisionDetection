import { Component, OnInit } from '@angular/core';
import { NbToastrService } from '@nebular/theme';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {

  inputFiles: FileList;
  selectedFiles: any[];
  uploadedFiles: any[];
  btnDelete: boolean;

  constructor(private toastrService: NbToastrService) { }

  ngOnInit() {
    this.uploadedFiles = [
      {
        url: 'https://media.sproutsocial.com/uploads/2017/02/10x-featured-social-media-image-size.png',
      },
      {
        url: 'https://media.sproutsocial.com/uploads/2017/02/10x-featured-social-media-image-size.png',
      },
      {
        url: 'https://media.sproutsocial.com/uploads/2017/02/10x-featured-social-media-image-size.png',
      },
      {
        url: 'https://media.sproutsocial.com/uploads/2017/02/10x-featured-social-media-image-size.png',
      }
    ];

    this.btnDelete = true;
  }

  onFilesChange(event) {
    this.inputFiles = event.target.files;
    console.log(this.inputFiles);
  }

  selectedChange() {
    this.btnDelete = true;
    for (const item of this.uploadedFiles) {
      if (item.selected === true) {
        this.btnDelete = false;
        break;
      }
    }
  }

  deleteSubmit() {
    for (let i = 0; i < this.uploadedFiles.length; i++) {
      if (this.uploadedFiles[i].selected === true) {
        this.uploadedFiles.splice(i, 1);
        i--;
      }
    }

    console.log(this.uploadedFiles);

    this.showToast('Deleted', 'Delete status');
  }

  showToast(title, message) {
    this.toastrService.show(message, title);
  }
}
