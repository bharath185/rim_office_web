package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

public class PayrollALLComponentCompactViewModel {
    @JsonProperty("EmpId")
    @JsonAlias({"empId", "EmpId"})
    private Integer empId;

    @JsonProperty("LoginId")
    @JsonAlias({"loginId", "LoginId"})
    private Integer loginId;

    @JsonProperty("EmpCode")
    @JsonAlias({"empCode", "EmpCode"})
    private String empCode;

    @JsonProperty("FirstName")
    @JsonAlias({"firstName", "FirstName"})
    private String firstName;

    @JsonProperty("MiddleName")
    @JsonAlias({"middleName", "MiddleName"})
    private String middleName;

    @JsonProperty("LastName")
    @JsonAlias({"lastName", "LastName"})
    private String lastName;

    @JsonProperty("PayoutTypeId")
    @JsonAlias({"payoutTypeId", "PayoutTypeId"})
    private Integer payoutTypeId;

    @JsonProperty("PayoutTypeName")
    @JsonAlias({"payoutTypeName", "PayoutTypeName"})
    private String payoutTypeName;

    @JsonProperty("FrequencyId")
    @JsonAlias({"frequencyId", "FrequencyId"})
    private Integer frequencyId;

    @JsonProperty("Frequency")
    @JsonAlias({"frequency", "Frequency"})
    private String frequency;

    @JsonProperty("SegmentId")
    @JsonAlias({"segmentId", "SegmentId"})
    private Integer segmentId;

    @JsonProperty("SegmentName")
    @JsonAlias({"segmentName", "SegmentName"})
    private String segmentName;

    @JsonProperty("ComponentId")
    @JsonAlias({"componentId", "ComponentId"})
    private Integer componentId;

    @JsonProperty("ComponentName")
    @JsonAlias({"componentName", "ComponentName"})
    private String componentName;

    @JsonProperty("ComponentCode")
    @JsonAlias({"componentCode", "ComponentCode"})
    private String componentCode;

    @JsonProperty("ComponentValue")
    @JsonAlias({"componentValue", "ComponentValue"})
    private String componentValue;

    @JsonProperty("LogicId")
    @JsonAlias({"logicId", "LogicId"})
    private Integer logicId;

    @JsonProperty("Percentage")
    @JsonAlias({"percentage", "Percentage"})
    private java.math.BigDecimal percentage;

    @JsonProperty("Value")
    @JsonAlias({"value", "Value"})
    private java.math.BigDecimal value;

    @JsonProperty("ComponentId1")
    @JsonAlias({"componentId1", "ComponentId1"})
    private Integer componentId1;

    @JsonProperty("ComponentName1")
    @JsonAlias({"componentName1", "ComponentName1"})
    private String componentName1;

    @JsonProperty("EffectiveFrom")
    @JsonAlias({"effectiveFrom", "EffectiveFrom"})
    private String effectiveFrom;

    @JsonProperty("EffectiveTo")
    @JsonAlias({"effectiveTo", "EffectiveTo"})
    private String effectiveTo;

    @JsonProperty("ConditionId")
    @JsonAlias({"conditionId", "ConditionId"})
    private Integer conditionId;

    @JsonProperty("ConditionExpression")
    @JsonAlias({"conditionExpression", "ConditionExpression"})
    private String conditionExpression;

    @JsonProperty("ConditionResultPFESI")
    @JsonAlias({"conditionResultPFESI", "ConditionResultPFESI"})
    private String conditionResultPFESI;

    @JsonProperty("LCtrue")
    @JsonAlias({"lCtrue", "LCtrue"})
    private Integer lCtrue;

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }
    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }
    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }
    public String getFirstName() { return firstName; }
    public void setFirstName(String firstName) { this.firstName = firstName; }
    public String getMiddleName() { return middleName; }
    public void setMiddleName(String middleName) { this.middleName = middleName; }
    public String getLastName() { return lastName; }
    public void setLastName(String lastName) { this.lastName = lastName; }
    public Integer getPayoutTypeId() { return payoutTypeId; }
    public void setPayoutTypeId(Integer payoutTypeId) { this.payoutTypeId = payoutTypeId; }
    public String getPayoutTypeName() { return payoutTypeName; }
    public void setPayoutTypeName(String payoutTypeName) { this.payoutTypeName = payoutTypeName; }
    public Integer getFrequencyId() { return frequencyId; }
    public void setFrequencyId(Integer frequencyId) { this.frequencyId = frequencyId; }
    public String getFrequency() { return frequency; }
    public void setFrequency(String frequency) { this.frequency = frequency; }
    public Integer getSegmentId() { return segmentId; }
    public void setSegmentId(Integer segmentId) { this.segmentId = segmentId; }
    public String getSegmentName() { return segmentName; }
    public void setSegmentName(String segmentName) { this.segmentName = segmentName; }
    public Integer getComponentId() { return componentId; }
    public void setComponentId(Integer componentId) { this.componentId = componentId; }
    public String getComponentName() { return componentName; }
    public void setComponentName(String componentName) { this.componentName = componentName; }
    public String getComponentCode() { return componentCode; }
    public void setComponentCode(String componentCode) { this.componentCode = componentCode; }
    public String getComponentValue() { return componentValue; }
    public void setComponentValue(String componentValue) { this.componentValue = componentValue; }
    public Integer getLogicId() { return logicId; }
    public void setLogicId(Integer logicId) { this.logicId = logicId; }
    public java.math.BigDecimal getPercentage() { return percentage; }
    public void setPercentage(java.math.BigDecimal percentage) { this.percentage = percentage; }
    public java.math.BigDecimal getValue() { return value; }
    public void setValue(java.math.BigDecimal value) { this.value = value; }
    public Integer getComponentId1() { return componentId1; }
    public void setComponentId1(Integer componentId1) { this.componentId1 = componentId1; }
    public String getComponentName1() { return componentName1; }
    public void setComponentName1(String componentName1) { this.componentName1 = componentName1; }
    public String getEffectiveFrom() { return effectiveFrom; }
    public void setEffectiveFrom(String effectiveFrom) { this.effectiveFrom = effectiveFrom; }
    public String getEffectiveTo() { return effectiveTo; }
    public void setEffectiveTo(String effectiveTo) { this.effectiveTo = effectiveTo; }
    public Integer getConditionId() { return conditionId; }
    public void setConditionId(Integer conditionId) { this.conditionId = conditionId; }
    public String getConditionExpression() { return conditionExpression; }
    public void setConditionExpression(String conditionExpression) { this.conditionExpression = conditionExpression; }
    public String getConditionResultPFESI() { return conditionResultPFESI; }
    public void setConditionResultPFESI(String conditionResultPFESI) { this.conditionResultPFESI = conditionResultPFESI; }
    public Integer getLCtrue() { return lCtrue; }
    public void setLCtrue(Integer lCtrue) { this.lCtrue = lCtrue; }
}
