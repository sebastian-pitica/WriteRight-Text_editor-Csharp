using System;
using System.Windows.Forms;

/**************************************************************************
 *                                                                        *
 *  File:        Interfaces.cs                                            *
 *  Copyright:   (c) 2023, Pitica Sebastian                               *
 *  Description: Fișierul conține interfețele, sub formele de Interface   *
 *  sau Abstract Class, utilizate în cadrul proiectului WriteRight.       *
 *  Scopul lor este de a pune în aplicare polimorfismul pentru clasele    *
 *  concrete, și de a oferi o structură bine definită acestora.           *
 *  Updated by:  Caulea Vasile                                            *
 *                                                                        *
 **************************************************************************/


namespace Interfaces
{
    /// <summary>
    /// Interfața pentru obiecte personalizate RichTextBox.
    /// Adaugă proprietăți ce pot fi utilizate pentru a gestiona fișierele asociate cu o casetă text,
    /// pentru a le accesa sau modifica în timpul rulării programului.
    /// </summary>
    /// <creator>Caulea Vasile</creator>
    public interface IRichTextBoxV2
    {
        string FilePath { set; get; }
        string FileName { get; }
        string FileType { get; }
        bool IsSaved { set; get; }
    }

    /// <summary>
    /// Interfața pentru obiecte personalizate de tip Text Editor.
    /// Adaugă proprietăți ce pot fi utilizate pentru a usura accesul la controale în timpul rulării programului.
    /// </summary>
    /// <creator>Caulea Vasile</creator>
    public interface ITextEditorControl
    {
        IRichTextBoxV2 RichTextBoxEditor { get; }
        RichTextBox RichTextBoxNumbering { get; }
        float ZoomFactor { get; set; }
    }

    /// <summary>
    /// Interfața pentru toate obiectele ce vor implementa funcționalitățile GUI. 
    /// Utilizată pentru implementarea Design Patternului Singleton împreună cu Command.
    /// </summary>
    /// <creator>Pitica Sebastian</creator>
    public abstract class SingletonCommand
    {
        protected SingletonCommand() { }
        public abstract void Execute();
        public static SingletonCommand GetCommandObj() { throw new NotImplementedException(); }
    }

    /// <summary>
    /// Interfața pentru toate obiectele de tip Singleton-Command care au drept target textBox-ul principal.
    /// </summary>
    /// <creator>Pitica Sebastian</creator>
    public abstract class MainTextBoxCommand : SingletonCommand
    {
        public abstract void SetTarget(IRichTextBoxV2 mainTextBox);
    }

    /// <summary>
    /// Interfața pentru toate obiectele de tip Singleton-Command care au drept target fereastra principală.
    /// </summary>
    /// <creator>Pitica Sebastian</creator>
    public abstract class MainWindowCommand : SingletonCommand
    {
        public abstract void SetTarget(Form mainWindow);
    }

    /// <summary>
    /// Interfata pentru toate obiectele de tip Singleton-Command care au drept target tab-ul de fisiere
    /// </summary>
    /// <creator>Caulea Vasile</creator>
    public abstract class TabControlCommand : SingletonCommand
    {
        public abstract void SetTarget(TabControl tabControl);
    }

    /// <summary>
    /// Interfata pentru toate obiectele de tip Singleton-Command care au drept target tab-ul de fisiere
    /// </summary>
    /// <creator>Pitica Sebastian</creator>
    public abstract class TextEditorControlCommand : SingletonCommand
    {
        public abstract void SetTarget(ITextEditorControl textEditorControl);
    }

    /// <summary>
    /// Interfața pentru un subiect de observat.Implementează design patternul Observer alături de Observer.
    /// </summary>
    /// <creator>Pitica Sebastian</creator>
    public interface ISubject
    {
        void Attach(IObserver observer);
        void NotifyObservers();
    }

    /// <summary>
    /// Interfața pentru un observator.Implementează design patternul Observer alături de Subject.
    /// </summary>
    /// <creator>Pitica Sebastian</creator>
    public interface IObserver
    {
        void UpdateObserver();
    }

}