﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DESVisualizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //DrawIt(0);
            graphics = this.CreateGraphics();
            this.Paint += new PaintEventHandler(pictureBox1_Paint);
            eventList = new List<Event>();
            arcList = new List<Arc>();
            textBox1.KeyDown += textBox1_Enter;
            textBox2.KeyDown += textBox2_Enter;
        }

        private System.Drawing.Graphics graphics;
        private StreamReader sr = new StreamReader("event_net_list1.txt");
        private StreamReader input = new StreamReader("input_file.txt");
        private List<Event> eventList;
        private List<Arc> arcList;
        int highlightedEvent = -1;
        int highlightedArc = -1;

        //private void drawArc(int current, int next)
        //{
        //    Rectangle rect = new Rectangle(25 + (current * 100), 35 - (next * 10), 100 + (next * 100), 35 + (next * 10));
        //    Pen blackPen = new Pen(Color.Black, 3);

        //    // Create start and sweep angles on ellipse.
        //    float startAngle = 0.0F;
        //    float sweepAngle = -180.0F;

        //    // Draw arc to screen.
        //    graphics.DrawArc(blackPen, rect, startAngle, sweepAngle);

        //}

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //drawArc(0);
            //drawArc(1, 0);
            //drawArc(0, 1);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Form Load");

        }

        private void DrawEvents_Click(object sender, EventArgs e)
        {
            foreach (Event ev in eventList)
            {
                ev.DrawEvent(graphics);
            }
            foreach (Arc arc in arcList)
            {
                arc.DrawArc(graphics);
            }
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            int edgeCount = 0;
            while (sr.Peek() >= 0)
            {
                string ev = sr.ReadLine();
                int eventName = ev[0] - '0';
                List<int> eventEdges = new List<int>();
                //Console.Write((char)eventName);
                for (int i = 0; i < ev.Length / 2; i++)
                {
                    eventEdges.Add(edgeCount++);
                    int edge = (int)char.GetNumericValue(ev[i * 2 + 2]);
                    arcList.Add(new Arc(eventName, edge));
                    //eventEdges.Add((int)Char.GetNumericValue(ev[i * 2 + 2]));
                }
                //foreach(int _edge in eventEdges)
                //{
                //    Console.WriteLine((int)_edge);
                //}
                //Console.WriteLine();
                eventList.Add(new Event(eventName, eventEdges));
            }
        }

        private void textBox1_Enter(object sender, KeyEventArgs e)
        {
            int textboxValue;
            if (int.TryParse(textBox1.Text, out textboxValue) && e.KeyCode == Keys.Enter)
            {
                if (Enumerable.Range(1, eventList.Count()).Contains(textboxValue))
                {
                    if (highlightedEvent != -1)
                        eventList.ElementAt(highlightedEvent).HighlightEvent(graphics, false);
                    highlightedEvent = int.Parse(textBox1.Text) - 1;
                    eventList.ElementAt(highlightedEvent).HighlightEvent(graphics, true);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void textBox2_Enter(object sender, KeyEventArgs e)
        {
            int textboxValue;
            if (int.TryParse(textBox2.Text, out textboxValue) && e.KeyCode == Keys.Enter)
            {
                if (Enumerable.Range(1, arcList.Count()).Contains(textboxValue))
                {
                    if (highlightedArc != -1)
                        arcList.ElementAt(highlightedArc).HighlightArc(graphics, false);
                    highlightedArc = int.Parse(textBox2.Text) - 1;
                    arcList.ElementAt(highlightedArc).HighlightArc(graphics, true);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }

        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (input.Peek() >= 0)
            {
                string ev = input.ReadLine();
                int inputType = ev[0] - '0';
                int modifiedElement = ev[2] - '0';
                if (inputType == 0)
                {
                    if (Enumerable.Range(1, eventList.Count()).Contains(modifiedElement))
                    {
                        if (highlightedArc != -1)
                        {
                            arcList.ElementAt(highlightedArc).HighlightArc(graphics, false);
                            highlightedArc = -1;
                        }

                        if (highlightedEvent != -1)
                            eventList.ElementAt(highlightedEvent).HighlightEvent(graphics, false);
                        highlightedEvent = modifiedElement - 1;
                        eventList.ElementAt(highlightedEvent).HighlightEvent(graphics, true);
                    }

                }
                else if (inputType == 1)
                {
                    if (Enumerable.Range(1, arcList.Count()).Contains(modifiedElement ))
                    {
                        if (highlightedArc != -1)
                            arcList.ElementAt(highlightedArc).HighlightArc(graphics, false);

                        if (highlightedEvent != -1)
                        {
                            highlightedArc = eventList.ElementAt(highlightedEvent).getNextArc(arcList, modifiedElement);
                            arcList.ElementAt(highlightedArc).HighlightArc(graphics, true);
                        }

                        else
                        {

                        }
                    }
                }
                else
                {

                }
            }
        }
    }
}

public class Event
{
    public Event(int _id, List<int> _edges) 
    {
        Console.WriteLine();
        Console.Write(_id);
        id = _id;
        edges = new List<int>();
        edges = _edges;
        location = id * 200;
        size = 100;
    }
    public void DrawEvent(Graphics graphics)
    {
        Console.Write((string)"Drawing Event");
        System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(
            (location), 100, size, size);
        graphics.DrawEllipse(System.Drawing.Pens.Black, rectangle);
        //graphics.DrawRectangle(System.Drawing.Pens.Black, rectangle);
        graphics.DrawString(id.ToString(), drawFont, drawBrush, location + 40, 137, drawFormat);
    }
    //public void DrawEdges(Graphics graphics)
    //{
    //    Console.Write((string)"Drawing Edges");
    //    foreach ( edge in edges)
    //    {
    //        edge.
    //    }
    //}
    public void HighlightEvent(System.Drawing.Graphics graphics, bool highlighted)
    {
        System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(
            (location), 100, size, size);
        if (highlighted == true)
            graphics.DrawEllipse(System.Drawing.Pens.Yellow, rectangle);
        else
            graphics.DrawEllipse(System.Drawing.Pens.Black, rectangle);
    }
    
    public int getNextArc(List<Arc> arcList, int nextEvent)
    {
        foreach (int edge in edges)
        {
            if (arcList.ElementAt(edge).next == nextEvent)
                return edge;
        }
        return -1;
    }
    public int id { get; }
    private List<int> edges;
    private int location;
    private int size;
    private System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
    private System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
    private System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();


}

public class Arc
{
    public static int nextID = 1;
    public Arc(int _current, int _next)
    {
        id = nextID++;
        //current = _current;
        //next = _next;
        current = _current;
        next = _next;
        difference = next - current;
    }
    public void DrawArc(System.Drawing.Graphics graphics)
    {
        //int height = 35 + (int)(Math.Pow(10, difference));
        int height = difference * 50;
        Pen blackPen = new Pen(Color.Black, 1);
        Rectangle rect;
        if (height > 0)
        {
            rect = new Rectangle(50 + (current * 200), 100 - height / 2, difference * 200, height);
            float startAngle = 0.0F;
            float sweepAngle = -180.0F;
            graphics.DrawArc(blackPen, rect, startAngle, sweepAngle);
            //graphics.DrawRectangle(blackPen, rect);
        }
        else
        {
            rect = new Rectangle(50 + (next * 200), 200 + height / 2, difference * -200, height * -1);
            float startAngle = 0.0F;
            float sweepAngle = 180.0F;
            graphics.DrawArc(blackPen, rect, startAngle, sweepAngle);
            //graphics.DrawRectangle(blackPen, rect);
        }
    }
    public void HighlightArc(System.Drawing.Graphics graphics, bool highlighted)
    {
        Pen pen;
        Rectangle rect;
        int height = difference * 50;
        if (highlighted == true)
            pen = new Pen(Color.Yellow, 1);
        else
            pen = new Pen(Color.Black, 1);
        if (height > 0)
        {
            rect = new Rectangle(50 + (current * 200), 100 - height / 2, difference * 200, height);
            float startAngle = 0.0F;
            float sweepAngle = -180.0F;
            graphics.DrawArc(pen, rect, startAngle, sweepAngle);
        }
        else
        {
            rect = new Rectangle(50 + (next * 200), 200 + height / 2, difference * -200, height * -1);
            float startAngle = 0.0F;
            float sweepAngle = 180.0F;
            graphics.DrawArc(pen, rect, startAngle, sweepAngle);
        }
    }
    public int id { get; set; }
    public int current { get; }
    public int next { get; }
    private int difference;
}