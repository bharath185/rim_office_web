package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.List;

public class SectionResponseViewModel {
    @JsonProperty("SectionId")
    private Integer sectionId;

    @JsonProperty("SectionName")
    private String sectionName;

    @JsonProperty("Components")
    private List<SalaryComponentViewModel> components;

    public Integer getSectionId() { return sectionId; }
    public void setSectionId(Integer sectionId) { this.sectionId = sectionId; }
    public String getSectionName() { return sectionName; }
    public void setSectionName(String sectionName) { this.sectionName = sectionName; }
    public List<SalaryComponentViewModel> getComponents() { return components; }
    public void setComponents(List<SalaryComponentViewModel> components) { this.components = components; }
}
