package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.Date;

public class PayrollALLComponentLogicConditionViewModel {

    @JsonProperty("ComponentId")
    private Integer componentId;

    @JsonProperty("LogicId")
    private Integer logicId;

    @JsonProperty("Percentage")
    private Double percentage;

    @JsonProperty("Value")
    private Double value;

    @JsonProperty("ComponentId1")
    private Integer componentId1;

    @JsonProperty("ComponentName1")
    private String componentName1;

    @JsonProperty("EffectiveFrom")
    private Date effectiveFrom;

    @JsonProperty("EffectiveTo")
    private Date effectiveTo;

    @JsonProperty("ConditionId")
    private Integer conditionId;

    @JsonProperty("ConditionExpression")
    private String conditionExpression;

    @JsonProperty("ConditionResultPFESI")
    private Boolean conditionResultPFESI;

    public Integer getComponentId() { return componentId; }
    public void setComponentId(Integer componentId) { this.componentId = componentId; }
    public Integer getLogicId() { return logicId; }
    public void setLogicId(Integer logicId) { this.logicId = logicId; }
    public Double getPercentage() { return percentage; }
    public void setPercentage(Double percentage) { this.percentage = percentage; }
    public Double getValue() { return value; }
    public void setValue(Double value) { this.value = value; }
    public Integer getComponentId1() { return componentId1; }
    public void setComponentId1(Integer componentId1) { this.componentId1 = componentId1; }
    public String getComponentName1() { return componentName1; }
    public void setComponentName1(String componentName1) { this.componentName1 = componentName1; }
    public Date getEffectiveFrom() { return effectiveFrom; }
    public void setEffectiveFrom(Date effectiveFrom) { this.effectiveFrom = effectiveFrom; }
    public Date getEffectiveTo() { return effectiveTo; }
    public void setEffectiveTo(Date effectiveTo) { this.effectiveTo = effectiveTo; }
    public Integer getConditionId() { return conditionId; }
    public void setConditionId(Integer conditionId) { this.conditionId = conditionId; }
    public String getConditionExpression() { return conditionExpression; }
    public void setConditionExpression(String conditionExpression) { this.conditionExpression = conditionExpression; }
    public Boolean getConditionResultPFESI() { return conditionResultPFESI; }
    public void setConditionResultPFESI(Boolean conditionResultPFESI) { this.conditionResultPFESI = conditionResultPFESI; }
}
