import { Component, OnInit } from '@angular/core';
import { FileUploadService, IResult, IKitData, IKit } from '../file-upload.service';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { trigger, state, transition, animate, style } from '@angular/animations';
import { TrackModel } from '../model/TrackModel';
import { InstrumentNamePipe } from '../instrument-name.pipe';
import { NoteNamePipe } from '../note-name.pipe';

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
  expandedTrack: any;
    chosenKit: any;
    kitInstruments: { [key: string]: IKit; };
  constructor(private fileUploadService: FileUploadService, private instrumentNamePipe: InstrumentNamePipe, private noteNamePipe: NoteNamePipe) { }

  kitData: Array<any>;
  kits: any;
  kitNotes: object;
  noteMap: object;
  model: TrackModel = {tracks:[]};

  ngOnInit() {

    this.fileUploadService.getDrumKits()
      .subscribe((result) => {
        if (!result)
          return;
        if (!result.data || result.progress != 100) {
          this.progress = result.progress;
          console.log(`Kits are ${this.progress}% loaded.`);
        } else {
          let data = result.data;
          this.kitData = <Array<any>>result.data['Notes'];
          this.kits = data.KitList;
          this.kitInstruments = data.Kits;
          this.noteMap = {};
          let kitNames = {};
          this.kitNotes = {};
          
          
        }
      });
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
          this.copyModel(result);
        }
      });
  }

  private copyModel(result: IResult) {
      this.midiFileAnalysis = result.data;
      this.model = {
        tracks: []
      }
      this.midiFileAnalysis.Tracks.forEach(track => {
        this.model.tracks.push({
          id: track.id,
          name: track.Name,
          channel: track.Channel,
          instrumentNumber: track.Instrument,
          instrumentName: this.instrumentNamePipe.transform(track.Instrument),
          instrumentFamily: this.instrumentNamePipe.transform(track.Instrument, "family"),
          noteMap: track.Notes.map(n => this.noteNamePipe.transform(n))
        })
      })
  }

  kitNoteForKit(instrument) {
    let notes = this.kitNotes[this.model.chosenKit][instrument];
    if (!notes)
      return {};
    notes.sort();
    return notes;
  }

  setKitOnTracks() {
    this.model.tracks.forEach(t => {
      t.chosenKitInstruments = this.kitInstruments[this.model.chosenKit] && this.kitInstruments[this.model.chosenKit].Instruments || {};
      // kits contain standard instrument names
      // so if we're changing kit, keep the same instrument
      // in the new kit
      const names = Object.keys(t.chosenKitInstruments);
      if (!t.kitInstrumentName || !names.includes(t.kitInstrumentName)) {
        names.forEach(k => {
          let kitFamily = t.chosenKitInstruments[k].Family;
          if (kitFamily === t.instrumentFamily || (kitFamily === "Drums" && t.channel === 10)) {
            t.kitInstrumentName = k;
            return false;
          }
        })

        // TODO: decide whether to set ignore as a default
        if (!t.kitInstrumentName) {
          t.kitInstrumentName = "ignore";
        }
      }
    })
  }

}
