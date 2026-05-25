package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.List;

public class UploadResult {
    @JsonProperty("TotalRecords")
    private int totalRecords;

    @JsonProperty("InsertedRecords")
    private int insertedRecords;

    @JsonProperty("FailedRecords")
    private int failedRecords;

    @JsonProperty("Exceptions")
    private List<AttendanceException> exceptions;

    public int getTotalRecords() { return totalRecords; }
    public void setTotalRecords(int totalRecords) { this.totalRecords = totalRecords; }

    public int getInsertedRecords() { return insertedRecords; }
    public void setInsertedRecords(int insertedRecords) { this.insertedRecords = insertedRecords; }

    public int getFailedRecords() { return failedRecords; }
    public void setFailedRecords(int failedRecords) { this.failedRecords = failedRecords; }

    public List<AttendanceException> getExceptions() { return exceptions; }
    public void setExceptions(List<AttendanceException> exceptions) { this.exceptions = exceptions; }
}
