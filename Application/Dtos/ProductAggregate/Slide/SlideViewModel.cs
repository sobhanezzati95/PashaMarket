﻿namespace Application.Dtos.ProductAggregate.Slide
{
    public class SlideViewModel
    {
        public long Id { get; set; }
        public string Picture { get; set; }
        public string Heading { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string CreationDate { get; set; }
    }
}