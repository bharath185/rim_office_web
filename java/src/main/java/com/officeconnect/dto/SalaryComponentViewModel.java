package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

public class SalaryComponentViewModel {
    @JsonProperty("SectionComponentId")
    private Integer sectionComponentId;

    @JsonProperty("ComponentId")
    private Integer componentId;

    @JsonProperty("ComponentName")
    private String componentName;

    @JsonProperty("ComponentCode")
    private String componentCode;

    @JsonProperty("SequenceNo")
    private Integer sequenceNo;

    @JsonProperty("ComponentValue")
    private String componentValue;

    public Integer getSectionComponentId() { return sectionComponentId; }
    public void setSectionComponentId(Integer sectionComponentId) { this.sectionComponentId = sectionComponentId; }
    public Integer getComponentId() { return componentId; }
    public void setComponentId(Integer componentId) { this.componentId = componentId; }
    public String getComponentName() { return componentName; }
    public void setComponentName(String componentName) { this.componentName = componentName; }
    public String getComponentCode() { return componentCode; }
    public void setComponentCode(String componentCode) { this.componentCode = componentCode; }
    public Integer getSequenceNo() { return sequenceNo; }
    public void setSequenceNo(Integer sequenceNo) { this.sequenceNo = sequenceNo; }
    public String getComponentValue() { return componentValue; }
    public void setComponentValue(String componentValue) { this.componentValue = componentValue; }
}
