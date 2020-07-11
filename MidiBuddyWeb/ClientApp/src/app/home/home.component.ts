import { Component, OnInit } from '@angular/core';
import { FileUploadService } from '../file-upload.service';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { trigger, state, transition, animate, style } from '@angular/animations';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0', display: 'none' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class HomeComponent implements OnInit {
  progress: number;
  midiFileAnalysis: any;
  displayedColumns: string[] = ['Name', 'Channel', 'Instrument', 'InstrumentFamily', 'KitInstrument'];
  expandedElement: any;
  constructor(private fileUploadService: FileUploadService) { }

  ngOnInit() {
  }

  onFileSelected(files: FileList) {
    if (!files || files.length === 0)
      return false;
    this.fileUploadService.postFile(files[0])
      .subscribe((result) => {
        if (!result)
          return;
        if (!result.data || result.progress != 100) {
          this.progress = result.progress;
          console.log(`File is ${this.progress}% loaded.`);
        } else {
          this.midiFileAnalysis = result.data;
        }
      });
  }

}
